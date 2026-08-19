using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FornecedoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public FornecedoresController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
    {
        return await _context.Fornecedores.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
    {
        var fornecedor = await _context.Fornecedores.FindAsync(id);

        if (fornecedor == null)
            return NotFound(new { mensagem = "Fornecedor não encontrado." });

        return fornecedor;
    }

    [HttpPost]
    public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
    {
        if (await _context.Fornecedores.AnyAsync(f => f.Cnpj == fornecedor.Cnpj))
            return BadRequest(new { mensagem = "Já existe um fornecedor cadastrado com este CNPJ." });

        _context.Fornecedores.Add(fornecedor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.Id }, fornecedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
    {
        if (id != fornecedor.Id)
            return BadRequest(new { mensagem = "ID do parâmetro não confere com o ID do fornecedor." });

        _context.Entry(fornecedor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Fornecedores.Any(e => e.Id == id))
                return NotFound(new { mensagem = "Fornecedor não encontrado." });

            throw;
        }

        return NoContent();
    }
}