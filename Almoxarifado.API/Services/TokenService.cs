using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Almoxarifado.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Almoxarifado.API.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration["JwtSettings:Secret"] ?? "SuaChaveSuperSecretaECompridaComMaisDe32CaracteresAqui123456!";
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JwtSettings:ExpiracaoEmHoras"] ?? "8")),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}