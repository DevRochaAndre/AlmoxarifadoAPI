namespace Almoxarifado.API.Models.Enums;

public enum StatusRequisicao
{
    Solicitada = 1,  // Criada pelo funcionário
    EmAnalise = 2,   // Em triagem / aprovação da chefia
    Aprovada = 3,    // Aprovada e com saldo EMPENHADO (reservado)
    Atendida = 4,    // Entregue e com baixa DEFINITIVA no estoque
    Rejeitada = 5,   // Recusada pela gestão
    Cancelada = 6    // Cancelada (libera o empenho se estivesse aprovada)
}