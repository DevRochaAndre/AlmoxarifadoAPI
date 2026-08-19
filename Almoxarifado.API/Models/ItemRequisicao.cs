using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Almoxarifado.API.Models;

public class ItemRequisicao
{
    public int Id { get; set; }

    public int RequisicaoId { get; set; }

    [JsonIgnore]
    public Requisicao? Requisicao { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int QuantidadeSolicitada { get; set; }

    public int QuantidadeAtendida { get; set; } = 0;
}