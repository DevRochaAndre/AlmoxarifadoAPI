using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models;

public class Item
{
    public int Id { get; set; }

    [Required]
    public int Codigo { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Descricao { get; set; }

    [MaxLength(80)]
    public string? Categoria { get; set; }

    [MaxLength(80)]
    public string? Marca { get; set; }

    [MaxLength(80)]
    public string? Modelo { get; set; }

    public DateTime? Validade { get; set; }

    [Required]
    public TipoProduto TipoProduto { get; set; } = TipoProduto.Consumivel;

    // Control do Estoque Triplo
    public int QuantidadeDisponivel { get; set; } = 0;
    public int QuantidadeEmpenhada { get; set; } = 0;

    // Propriedade calculada em memória (Visão Total Estratégica)
    public int QuantidadeTotal => QuantidadeDisponivel + QuantidadeEmpenhada;

    public bool Ativo { get; set; } = true;
}