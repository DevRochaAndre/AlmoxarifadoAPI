namespace Almoxarifado.API.DTOs;

public class ResumoDashboardDto
{
    public int TotalItensCadastrados { get; set; }
    public int TotalItensDisponiveis { get; set; }
    public int TotalItensEmpenhados { get; set; }
    public int ItensComEstoqueBaixo { get; set; }
    public int RequisicoesPendentes { get; set; }
}

public class ItemEmposseDto
{
    public int FuncionarioId { get; set; }
    public string NomeFuncionario { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int ItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public string CodigoItem { get; set; } = string.Empty;
    public int QuantidadeEmUso { get; set; }
}

public class ItemEstoqueBaixoDto
{
    public int ItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public int QuantidadeDisponivel { get; set; }
    public int LimiteMinimoRecomendado { get; set; }
}