using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntradasNotasFiscaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public EntradasNotasFiscaisController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntradaNotaFiscal>>> GetEntradas()
    {
        return await _context.EntradasNotasFiscais
            .Include(e => e.Fornecedor)
            .Include(e => e.Itens)
                .ThenInclude(i => i.Item)
            .OrderByDescending(e => e.DataEntrada)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntradaNotaFiscal>> GetEntrada(int id)
    {
        var entrada = await _context.EntradasNotasFiscais
            .Include(e => e.Fornecedor)
            .Include(e => e.Itens)
                .ThenInclude(i => i.Item)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entrada == null)
            return NotFound(new { mensagem = "Entrada de nota fiscal não encontrada." });

        return entrada;
    }

    [HttpPost]
    public async Task<ActionResult<EntradaNotaFiscal>> PostEntrada(EntradaNotaFiscal entrada)
    {
        if (entrada.Itens == null || !entrada.Itens.Any())
            return BadRequest(new { mensagem = "A nota fiscal deve conter pelo menos um item." });

        var fornecedorExiste = await _context.Fornecedores.AnyAsync(f => f.Id == entrada.FornecedorId);
        if (!fornecedorExiste)
            return BadRequest(new { mensagem = "Fornecedor informado não foi encontrado." });

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            entrada.DataEntrada = DateTime.Now;

            foreach (var itemNota in entrada.Itens)
            {
                var itemEstoque = await _context.Itens.FindAsync(itemNota.ItemId);
                if (itemEstoque == null)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { mensagem = $"Item com ID {itemNota.ItemId} não encontrado no cadastro." });
                }

                if (itemNota.Quantidade <= 0)
                {
                    await transaction.RollbackAsync();
                    // Navegação segura com operador ?. e fallback nulo para evitar CS8602
                    var nomeItem = itemEstoque.Nome ?? $"ID {itemNota.ItemId}";
                    return BadRequest(new { mensagem = $"A quantidade para o item '{nomeItem}' deve ser maior que zero." });
                }

                // Incrementa a quantidade disponível na prateleira ao dar entrada na Nota Fiscal
                itemEstoque.QuantidadeDisponivel += itemNota.Quantidade;
            }

            _context.EntradasNotasFiscais.Add(entrada);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetEntrada), new { id = entrada.Id }, entrada);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { mensagem = "Erro ao dar entrada na nota fiscal.", detalhe = ex.Message });
        }
    }
}