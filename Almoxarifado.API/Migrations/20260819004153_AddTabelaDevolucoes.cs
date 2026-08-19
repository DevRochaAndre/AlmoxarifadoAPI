using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Almoxarifado.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelaDevolucoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devolucoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequisicaoId = table.Column<int>(type: "int", nullable: false),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QuantidadeDevolvida = table.Column<int>(type: "int", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CondicaoItem = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devolucoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devolucoes_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devolucoes_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devolucoes_Requisicoes_RequisicaoId",
                        column: x => x.RequisicaoId,
                        principalTable: "Requisicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucoes_FuncionarioId",
                table: "Devolucoes",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucoes_ItemId",
                table: "Devolucoes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucoes_RequisicaoId",
                table: "Devolucoes",
                column: "RequisicaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devolucoes");
        }
    }
}
