using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class add_turno_personal_opcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "PersonalOpcId",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_PersonalOpcId",
                table: "Turnos",
                column: "PersonalOpcId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Personal_PersonalOpcId",
                table: "Turnos",
                column: "PersonalOpcId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Personal_PersonalOpcId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_PersonalOpcId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "PersonalOpcId",
                table: "Turnos");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
