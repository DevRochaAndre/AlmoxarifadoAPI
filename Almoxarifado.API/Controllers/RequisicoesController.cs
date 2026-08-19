using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Almoxarifado.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequisicoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RequisicoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Requisicao>>> GetRequisicoes()
    {
        return await _context.Requisicoes
            .Include(r => r.Funcionario)
            .Include(r => r.Itens)
                .ThenInclude(i => i.Item)
            .OrderByDescending(r => r.DataSolicitacao)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Requisicao>> GetRequisicao(int id)
    {
        var requisicao = await _context.Requisicoes
            .Include(r => r.Funcionario)
            .Include(r => r.Itens)
                .ThenInclude(i => i.Item)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (requisicao == null)
            return NotFound(new { mensagem = "Requisição não encontrada." });

        return requisicao;
    }

    [HttpPost]
    public async Task<ActionResult<Requisicao>> PostRequisicao(Requisicao requisicao)
    {
        if (requisicao.Itens == null || !requisicao.Itens.Any())
            return BadRequest(new { mensagem = "A requisição deve conter pelo menos um item." });

        var funcionarioExiste = await _context.Funcionarios.AnyAsync(f => f.Id == requisicao.FuncionarioId);
        if (!funcionarioExiste)
            return BadRequest(new { mensagem = "Funcionário solicitante não encontrado." });

        foreach (var itemReq in requisicao.Itens)
        {
            var itemEstoque = await _context.Itens.FindAsync(itemReq.ItemId);
            if (itemEstoque == null)
                return BadRequest(new { mensagem = $"Item com ID {itemReq.ItemId} não existe no cadastro." });

            if (itemReq.QuantidadeSolicitada <= 0)
                return BadRequest(new { mensagem = $"A quantidade solicitada para o item {itemEstoque.Nome} deve ser maior que zero." });
        }

        requisicao.Status = StatusRequisicao.Solicitada;
        requisicao.DataSolicitacao = DateTime.Now;

        _context.Requisicoes.Add(requisicao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRequisicao), new { id = requisicao.Id }, requisicao);
    }

    [HttpPut("{id}/aprovar")]
    public async Task<IActionResult> AprovarRequisicao(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var requisicao = await _context.Requisicoes
                .Include(r => r.Itens)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requisicao == null)
                return NotFound(new { mensagem = "Requisição não encontrada." });

            if (requisicao.Status != StatusRequisicao.Solicitada && requisicao.Status != StatusRequisicao.EmAnalise)
                return BadRequest(new { mensagem = $"Não é possível aprovar uma requisição com status '{requisicao.Status}'." });

            // Valida apenas a disponibilidade do estoque na prateleira
            foreach (var itemReq in requisicao.Itens)
            {
                var itemEstoque = await _context.Itens.FindAsync(itemReq.ItemId);

                if (itemEstoque == null)
                    return BadRequest(new { mensagem = $"Item ID {itemReq.ItemId} não encontrado." });

                if (itemEstoque.QuantidadeDisponivel < itemReq.QuantidadeSolicitada)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new
                    {
                        mensagem = $"Saldo insuficiente para o item '{itemEstoque.Nome}'. Disponível na prateleira: {itemEstoque.QuantidadeDisponivel} | Solicitado: {itemReq.QuantidadeSolicitada}."
                    });
                }
            }

            requisicao.Status = StatusRequisicao.Aprovada;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { mensagem = "Requisição aprovada com sucesso!", requisicao });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { mensagem = "Erro ao processar a aprovação da requisição.", detalhe = ex.Message });
        }
    }

    [HttpPut("{id}/atender")]
    public async Task<IActionResult> AtenderRequisicao(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var requisicao = await _context.Requisicoes
                .Include(r => r.Itens)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requisicao == null)
                return NotFound(new { mensagem = "Requisição não encontrada." });

            if (requisicao.Status != StatusRequisicao.Aprovada)
                return BadRequest(new { mensagem = "Apenas requisições com status 'Aprovada' podem ser atendidas/entregues." });

            foreach (var itemReq in requisicao.Itens)
            {
                var itemEstoque = await _context.Itens.FindAsync(itemReq.ItemId);

                if (itemEstoque == null)
                    return BadRequest(new { mensagem = $"Item ID {itemReq.ItemId} não encontrado." });

                if (itemEstoque.QuantidadeDisponivel < itemReq.QuantidadeSolicitada)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new
                    {
                        mensagem = $"Saldo insuficiente na prateleira para entregar o item '{itemEstoque.Nome}'. Disponível: {itemEstoque.QuantidadeDisponivel} | Solicitado: {itemReq.QuantidadeSolicitada}."
                    });
                }

                // Regra explícita pelo TipoProduto:
                if (itemEstoque.TipoProduto == TipoProduto.Consumivel)
                {
                    // Consumível: Baixa definitiva do saldo disponível
                    itemEstoque.QuantidadeDisponivel -= itemReq.QuantidadeSolicitada;
                }
                else if (itemEstoque.TipoProduto == TipoProduto.Retornavel)
                {
                    // Retornável: Sai do 'Disponível' e entra no 'Empenhado' (controle de posse/auditoria)
                    itemEstoque.QuantidadeDisponivel -= itemReq.QuantidadeSolicitada;
                    itemEstoque.QuantidadeEmpenhada += itemReq.QuantidadeSolicitada;
                }

                itemReq.QuantidadeAtendida = itemReq.QuantidadeSolicitada;
            }

            requisicao.Status = StatusRequisicao.Atendida;
            requisicao.DataAtendimento = DateTime.Now;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { mensagem = "Requisição atendida/entregue com sucesso!", requisicao });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { mensagem = "Erro ao processar o atendimento da requisição.", detalhe = ex.Message });
        }
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> CancelarRequisicao(int id)
    {
        var requisicao = await _context.Requisicoes.FindAsync(id);

        if (requisicao == null)
            return NotFound(new { mensagem = "Requisição não encontrada." });

        if (requisicao.Status == StatusRequisicao.Atendida)
            return BadRequest(new { mensagem = "Não é possível cancelar uma requisição que já foi atendida/entregue." });

        if (requisicao.Status == StatusRequisicao.Cancelada)
            return BadRequest(new { mensagem = "Esta requisição já está cancelada." });

        requisicao.Status = StatusRequisicao.Cancelada;
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Requisição cancelada com sucesso!", requisicao });
    }

   
}