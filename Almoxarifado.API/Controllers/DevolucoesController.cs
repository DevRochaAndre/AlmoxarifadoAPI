using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DevolucoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Devolucao>>> GetDevolucoes()
    {
        return await _context.Devolucoes
            .Include(d => d.Funcionario)
            .Include(d => d.Item)
            .Include(d => d.Requisicao)
            .OrderByDescending(d => d.DataDevolucao)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Devolucao>> GetDevolucao(int id)
    {
        var devolucao = await _context.Devolucoes
            .Include(d => d.Funcionario)
            .Include(d => d.Item)
            .Include(d => d.Requisicao)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (devolucao == null)
            return NotFound(new { mensagem = "Registro de devolução não encontrado." });

        return devolucao;
    }

    [HttpPost]
    public async Task<ActionResult<Devolucao>> RegistrarDevolucao(Devolucao devolucao)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var item = await _context.Itens.FindAsync(devolucao.ItemId);
            if (item == null)
                return BadRequest(new { mensagem = "Item não encontrado." });

            if (item.TipoProduto != TipoProduto.Retornavel)
                return BadRequest(new { mensagem = "Apenas itens do tipo 'Retornavel' podem passar por processo de devolução." });

            if (devolucao.QuantidadeDevolvida <= 0)
                return BadRequest(new { mensagem = "A quantidade devolvida deve ser maior que zero." });

            if (item.QuantidadeEmpenhada < devolucao.QuantidadeDevolvida)
                return BadRequest(new
                {
                    mensagem = $"A quantidade a devolver ({devolucao.QuantidadeDevolvida}) é maior do que a quantidade em uso/empenhada ({item.QuantidadeEmpenhada})."
                });

            var funcionarioExiste = await _context.Funcionarios.AnyAsync(f => f.Id == devolucao.FuncionarioId);
            if (!funcionarioExiste)
                return BadRequest(new { mensagem = "Funcionário não encontrado." });

            var requisicaoExiste = await _context.Requisicoes.AnyAsync(r => r.Id == devolucao.RequisicaoId);
            if (!requisicaoExiste)
                return BadRequest(new { mensagem = "Requisição de origem não encontrada." });

            // Atualiza os saldos no estoque (Transfere do Empenhado de volta para Disponível na prateleira)
            item.QuantidadeEmpenhada -= devolucao.QuantidadeDevolvida;
            item.QuantidadeDisponivel += devolucao.QuantidadeDevolvida;

            devolucao.DataDevolucao = DateTime.Now;

            _context.Devolucoes.Add(devolucao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetDevolucao), new { id = devolucao.Id }, devolucao);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { mensagem = "Erro ao registrar a devolução.", detalhe = ex.Message });
        }
    }
}