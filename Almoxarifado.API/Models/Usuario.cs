using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models;

public enum PerfilUsuario
{
    Funcionario = 1,
    Almoxarife = 2,
    Admin = 3
}

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;

    public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Funcionario;

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.Now;
}