using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItensController : ControllerBase
{
    private readonly AppDbContext _context;

    public ItensController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/itens
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItens()
    {
        return await _context.Itens.ToListAsync();
    }

    // GET: api/itens/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _context.Itens.FindAsync(id);

        if (item == null)
        {
            return NotFound(new { mensagem = "Item não encontrado." });
        }

        return item;
    }

    // POST: api/itens
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(Item item)
    {
        // Valida se o código já existe
        var codigoExistente = await _context.Itens.AnyAsync(i => i.Codigo == item.Codigo);
        if (codigoExistente)
        {
            return BadRequest(new { mensagem = $"Já existe um item cadastrado com o código {item.Codigo}." });
        }

        _context.Itens.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    // PUT: api/itens/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, Item item)
    {
        if (id != item.Id)
        {
            return BadRequest(new { mensagem = "O ID da URL não confere com o ID do objeto." });
        }

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Itens.AnyAsync(i => i.Id == id))
            {
                return NotFound(new { mensagem = "Item não encontrado." });
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPut("{id}/ajustar-saldos")]
    public async Task<IActionResult> AjustarSaldosItem(int id, [FromQuery] int quantidadeDisponivel, [FromQuery] int quantidadeEmpenhada)
    {
        var item = await _context.Itens.FindAsync(id);
        if (item == null)
            return NotFound(new { mensagem = "Item não encontrado." });

        if (quantidadeDisponivel < 0 || quantidadeEmpenhada < 0)
            return BadRequest(new { mensagem = "As quantidades não podem ser negativas." });

        // Atualiza apenas as propriedades com setter
        item.QuantidadeDisponivel = quantidadeDisponivel;
        item.QuantidadeEmpenhada = quantidadeEmpenhada;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = $"Saldos do item '{item.Nome}' ajustados com sucesso!",
            item
        });
    }
}