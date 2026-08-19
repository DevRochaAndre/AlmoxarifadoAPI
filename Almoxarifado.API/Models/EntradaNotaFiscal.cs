using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Almoxarifado.API.Models;

public class EntradaNotaFiscal
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string NumeroNota { get; set; } = string.Empty;

    [MaxLength(44)]
    public string? ChaveAcesso { get; set; }

    public DateTime DataEmissao { get; set; } = DateTime.Now;
    public DateTime DataEntrada { get; set; } = DateTime.Now;

    public int FornecedorId { get; set; }
    public Fornecedor? Fornecedor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotalNota { get; set; }

    public List<ItemEntradaNotaFiscal> Itens { get; set; } = new();
}