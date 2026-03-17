using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecordatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistroId",
                table: "Recordatorios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultadoObservacion",
                table: "Recordatorios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recordatorios_RegistroId",
                table: "Recordatorios",
                column: "RegistroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recordatorios_Registros_RegistroId",
                table: "Recordatorios",
                column: "RegistroId",
                principalTable: "Registros",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recordatorios_Registros_RegistroId",
                table: "Recordatorios");

            migrationBuilder.DropIndex(
                name: "IX_Recordatorios_RegistroId",
                table: "Recordatorios");

            migrationBuilder.DropColumn(
                name: "RegistroId",
                table: "Recordatorios");

            migrationBuilder.DropColumn(
                name: "ResultadoObservacion",
                table: "Recordatorios");
        }
    }
}
