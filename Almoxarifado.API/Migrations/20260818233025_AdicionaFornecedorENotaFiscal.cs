using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Almoxarifado.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaFornecedorENotaFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cnpj = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeFantasia = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntradasNotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroNota = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChaveAcesso = table.Column<string>(type: "varchar(44)", maxLength: 44, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataEmissao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    ValorTotalNota = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasNotasFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradasNotasFiscais_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItensEntradasNotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntradaNotaFiscalId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensEntradasNotasFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensEntradasNotasFiscais_EntradasNotasFiscais_EntradaNotaFi~",
                        column: x => x.EntradaNotaFiscalId,
                        principalTable: "EntradasNotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensEntradasNotasFiscais_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasNotasFiscais_FornecedorId",
                table: "EntradasNotasFiscais",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensEntradasNotasFiscais_EntradaNotaFiscalId",
                table: "ItensEntradasNotasFiscais",
                column: "EntradaNotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensEntradasNotasFiscais_ItemId",
                table: "ItensEntradasNotasFiscais",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensEntradasNotasFiscais");

            migrationBuilder.DropTable(
                name: "EntradasNotasFiscais");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
