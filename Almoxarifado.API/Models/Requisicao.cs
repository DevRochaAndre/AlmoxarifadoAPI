using Almoxarifado.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Almoxarifado.API.Models;

public class Requisicao
{
    public int Id { get; set; }

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;

    public DateTime? DataAtendimento { get; set; }

    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public StatusRequisicao Status { get; set; } = StatusRequisicao.Solicitada;

    [MaxLength(250)]
    public string? Observacao { get; set; }

    public List<ItemRequisicao> Itens { get; set; } = new();
}