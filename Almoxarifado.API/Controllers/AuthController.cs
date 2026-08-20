using Almoxarifado.API.Data;
using Almoxarifado.API.DTOs;
using Almoxarifado.API.Models;
using Almoxarifado.API.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Almoxarifado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<RespostaAuthDto>> Registrar(RegistroUsuarioDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(new { mensagem = "Já existe um usuário cadastrado com este e-mail." });

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Perfil = (PerfilUsuario)dto.Perfil
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var token = _tokenService.GerarToken(usuario);

        return Ok(new RespostaAuthDto
        {
            Token = token,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil.ToString()
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<RespostaAuthDto>> Login(LoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });

        if (!usuario.Ativo)
            return Unauthorized(new { mensagem = "Usuário inativo no sistema." });

        var token = _tokenService.GerarToken(usuario);

        return Ok(new RespostaAuthDto
        {
            Token = token,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil.ToString()
        });
    }
}