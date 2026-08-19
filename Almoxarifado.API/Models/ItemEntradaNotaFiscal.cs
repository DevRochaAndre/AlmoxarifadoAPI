using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Almoxarifado.API.Models;

public class ItemEntradaNotaFiscal
{
    public int Id { get; set; }

    public int EntradaNotaFiscalId { get; set; }

    [JsonIgnore]
    public EntradaNotaFiscal? EntradaNotaFiscal { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoUnitario { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal => Quantidade * PrecoUnitario;
}