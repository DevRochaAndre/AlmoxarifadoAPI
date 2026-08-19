using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models;

public class Fornecedor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(18)] // Formato: 00.000.000/0001-00
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string RazaoSocial { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? NomeFantasia { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public bool Ativo { get; set; } = true;
}