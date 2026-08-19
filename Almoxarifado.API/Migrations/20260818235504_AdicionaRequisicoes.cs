using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Almoxarifado.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaRequisicoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requisicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtendimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisicoes_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItensRequisicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequisicaoId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QuantidadeSolicitada = table.Column<int>(type: "int", nullable: false),
                    QuantidadeAtendida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensRequisicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensRequisicoes_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensRequisicoes_Requisicoes_RequisicaoId",
                        column: x => x.RequisicaoId,
                        principalTable: "Requisicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRequisicoes_ItemId",
                table: "ItensRequisicoes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRequisicoes_RequisicaoId",
                table: "ItensRequisicoes",
                column: "RequisicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicoes_FuncionarioId",
                table: "Requisicoes",
                column: "FuncionarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensRequisicoes");

            migrationBuilder.DropTable(
                name: "Requisicoes");
        }
    }
}
