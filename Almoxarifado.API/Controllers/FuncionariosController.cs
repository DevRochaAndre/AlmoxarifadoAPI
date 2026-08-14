using Almoxarifado.API.Data;
using Almoxarifado.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/funcionarios (Listar todos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.ToListAsync();
            return Ok(funcionarios);
        }

        // 2. GET: api/funcionarios/101 (Buscar pela MATRÍCULA do funcionário)
        [HttpGet("{matricula:int}")]
        public async Task<ActionResult<Funcionario>> GetFuncionarioPorMatricula(int matricula)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Matricula == matricula);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com a matrícula {matricula} não foi encontrado."); // HTTP 404
            }

            return Ok(funcionario); // HTTP 200
        }

        // 3. POST: api/funcionarios (Cadastrar)
        [HttpPost]
        public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
        {
            // Valida se a MATRÍCULA já existe
            bool matriculaExiste = await _context.Funcionarios.AnyAsync(f => f.Matricula == funcionario.Matricula);
            if (matriculaExiste)
            {
                return BadRequest($"Já existe um funcionário cadastrado com a matrícula {funcionario.Matricula}.");
            }

            // Valida se o CPF já existe
            bool cpfExiste = await _context.Funcionarios.AnyAsync(f => f.Cpf == funcionario.Cpf);
            if (cpfExiste)
            {
                return BadRequest("Já existe um funcionário cadastrado com este CPF.");
            }

            funcionario.DataCadastro = DateTime.UtcNow;
            funcionario.Ativo = true;

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFuncionarioPorMatricula), new { matricula = funcionario.Matricula }, funcionario);
        }

        // 4. PUT: api/funcionarios/101 (Atualizar dados pela MATRÍCULA)
        [HttpPut("{matricula:int}")]
        public async Task<IActionResult> PutFuncionario(int matricula, Funcionario funcionario)
        {
            if (matricula != funcionario.Matricula)
            {
                return BadRequest("A Matrícula informada na URL não confere com a Matrícula do objeto enviado.");
            }

            var funcionarioExistente = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Matricula == matricula);

            if (funcionarioExistente == null)
            {
                return NotFound($"Funcionário com a matrícula {matricula} não foi encontrado para atualização.");
            }

            // Atualiza os campos permitidos
            funcionarioExistente.Nome = funcionario.Nome;
            funcionarioExistente.Email = funcionario.Email;
            funcionarioExistente.Cargo = funcionario.Cargo;
            funcionarioExistente.Ativo = funcionario.Ativo;

            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204
        }

        // 5. DELETE: api/funcionarios/101 (Inativação Lógica pela MATRÍCULA)
        [HttpDelete("{matricula:int}")]
        public async Task<IActionResult> DeleteFuncionario(int matricula)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Matricula == matricula);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com a matrícula {matricula} não foi encontrado.");
            }

            // Soft Delete (Inativação lógica)
            funcionario.Ativo = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Funcionário '{funcionario.Nome}' (Matrícula: {funcionario.Matricula}) foi inativado com sucesso." });
        }
    }
}