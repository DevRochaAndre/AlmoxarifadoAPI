using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models;

public class Devolucao
{
    public int Id { get; set; }

    [Required]
    public int RequisicaoId { get; set; }
    public Requisicao? Requisicao { get; set; }

    [Required]
    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    [Required]
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int QuantidadeDevolvida { get; set; }

    public DateTime DataDevolucao { get; set; } = DateTime.Now;

    [MaxLength(255)]
    public string? CondicaoItem { get; set; } // Ex: "Em perfeito estado", "Tela arranhada", etc.

    [MaxLength(255)]
    public string? Observacao { get; set; }
}