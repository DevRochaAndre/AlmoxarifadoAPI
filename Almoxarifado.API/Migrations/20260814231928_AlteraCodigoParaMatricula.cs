using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Almoxarifado.API.Migrations
{
    /// <inheritdoc />
    public partial class AlteraCodigoParaMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "Funcionarios",
                newName: "Matricula");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionarios_Codigo",
                table: "Funcionarios",
                newName: "IX_Funcionarios_Matricula");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Matricula",
                table: "Funcionarios",
                newName: "Codigo");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionarios_Matricula",
                table: "Funcionarios",
                newName: "IX_Funcionarios_Codigo");
        }
    }
}
