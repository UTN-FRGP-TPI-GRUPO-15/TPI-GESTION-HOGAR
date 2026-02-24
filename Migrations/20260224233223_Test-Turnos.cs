using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class TestTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalTurno");

            migrationBuilder.DropColumn(
                name: "Horas",
                table: "Turnos");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_PersonalId",
                table: "Turnos",
                column: "PersonalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Personal_PersonalId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_PersonalId",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "Horas",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PersonalTurno",
                columns: table => new
                {
                    PersonalID = table.Column<int>(type: "int", nullable: false),
                    TurnoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalTurno", x => new { x.PersonalID, x.TurnoID });
                    table.ForeignKey(
                        name: "FK_PersonalTurno_Personal_PersonalID",
                        column: x => x.PersonalID,
                        principalTable: "Personal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalTurno_Turnos_TurnoID",
                        column: x => x.TurnoID,
                        principalTable: "Turnos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTurno_TurnoID",
                table: "PersonalTurno",
                column: "TurnoID");
        }
    }
}
