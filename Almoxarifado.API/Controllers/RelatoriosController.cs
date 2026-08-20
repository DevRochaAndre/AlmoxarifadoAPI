using Almoxarifado.API.Data;
using Almoxarifado.API.DTOs;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly AppDbContext _context;

    public RelatoriosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Relatorios/dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<ResumoDashboardDto>> GetResumoDashboard()
    {
        var totalItens = await _context.Itens.CountAsync(i => i.Ativo);
        var disponiveis = await _context.Itens.Where(i => i.Ativo).SumAsync(i => i.QuantidadeDisponivel);
        var empenhados = await _context.Itens.Where(i => i.Ativo).SumAsync(i => i.QuantidadeEmpenhada);

        // Exemplo: considera estoque baixo itens consumíveis com menos de 5 unidades disponíveis
        var estoqueBaixo = await _context.Itens.CountAsync(i => i.Ativo && i.TipoProduto == TipoProduto.Consumivel && i.QuantidadeDisponivel <= 5);

        // Status 1 = Pendente (Ajuste conforme o Enum StatusRequisicao do seu projeto)
        var requisicoesPendentes = await _context.Requisicoes.CountAsync(r => (int)r.Status == 1);

        var resumo = new ResumoDashboardDto
        {
            TotalItensCadastrados = totalItens,
            TotalItensDisponiveis = disponiveis,
            TotalItensEmpenhados = empenhados,
            ItensComEstoqueBaixo = estoqueBaixo,
            RequisicoesPendentes = requisicoesPendentes
        };

        return Ok(resumo);
    }

    // GET: api/Relatorios/itens-em-posse
    [HttpGet("itens-em-posse")]
    public async Task<ActionResult<IEnumerable<ItemEmposseDto>>> GetItensEmPosse()
    {
        // 1. Busca todas as devoluções já registradas no sistema
        var devolucoes = await _context.Devolucoes.ToListAsync();

        // 2. Busca todas as requisições atendidas com produtos retornáveis
        var requisicoes = await _context.Requisicoes
            .Include(r => r.Funcionario)
            .Include(r => r.Itens)
                .ThenInclude(ri => ri.Item)
            .Where(r => r.Itens.Any(i => i.Item != null && i.Item.TipoProduto == TipoProduto.Retornavel))
            .ToListAsync();

        var listaEmPosse = new List<ItemEmposseDto>();

        foreach (var req in requisicoes)
        {
            foreach (var itemReq in req.Itens.Where(i => i.Item != null && i.Item.TipoProduto == TipoProduto.Retornavel))
            {
                // Soma quanto já foi devolvido especificamente desta requisição para este item
                var qtdDevolvida = devolucoes
                    .Where(d => d.RequisicaoId == req.Id && d.ItemId == itemReq.ItemId)
                    .Sum(d => d.QuantidadeDevolvida);

                var qtdAindaEmPosse = itemReq.QuantidadeAtendida - qtdDevolvida;

                // Se o funcionário ainda possui ao menos 1 unidade em mãos, adiciona ao relatório
                if (qtdAindaEmPosse > 0)
                {
                    listaEmPosse.Add(new ItemEmposseDto
                    {
                        FuncionarioId = req.FuncionarioId,
                        NomeFuncionario = req.Funcionario != null ? req.Funcionario.Nome : "Não informado",
                        Cargo = req.Funcionario != null ? req.Funcionario.Cargo : "N/A",
                        ItemId = itemReq.ItemId,
                        NomeItem = itemReq.Item != null ? itemReq.Item.Nome : "N/A",
                        CodigoItem = itemReq.Item != null ? itemReq.Item.Codigo.ToString() : "N/A",
                        QuantidadeEmUso = qtdAindaEmPosse
                    });
                }
            }
        }

        return Ok(listaEmPosse);
    }

    // GET: api/Relatorios/estoque-baixo
    [HttpGet("estoque-baixo")]
    public async Task<ActionResult<IEnumerable<ItemEstoqueBaixoDto>>> GetItensEstoqueBaixo([FromQuery] int limite = 5)
    {
        var itensCriticos = await _context.Itens
            .Where(i => i.Ativo && i.TipoProduto == TipoProduto.Consumivel && i.QuantidadeDisponivel <= limite)
            .Select(i => new ItemEstoqueBaixoDto
            {
                ItemId = i.Id,
                NomeItem = i.Nome,
                QuantidadeDisponivel = i.QuantidadeDisponivel,
                LimiteMinimoRecomendado = limite
            })
            .ToListAsync();

        return Ok(itensCriticos);
    }
}