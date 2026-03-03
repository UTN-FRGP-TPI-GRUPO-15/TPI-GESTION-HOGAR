using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class update_Personal_Usuario_Rol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTurno_Personal_PersonalID",
                table: "PersonalTurno");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTurno_Turnos_TurnoID",
                table: "PersonalTurno");

            migrationBuilder.RenameColumn(
                name: "PersonalID",
                table: "PersonalTurno",
                newName: "PersonalId");

            migrationBuilder.RenameColumn(
                name: "TurnoID",
                table: "PersonalTurno",
                newName: "TurnosID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTurno_TurnoID",
                table: "PersonalTurno",
                newName: "IX_PersonalTurno_TurnosID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Personal",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Personal",
                newName: "Activo");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Personal",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Personal",
                keyColumn: "Id",
                keyValue: 1,
                column: "DNI",
                value: "25123456");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "lgarcia@test.com");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTurno_Personal_PersonalId",
                table: "PersonalTurno",
                column: "PersonalId",
                principalTable: "Personal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTurno_Turnos_TurnosID",
                table: "PersonalTurno",
                column: "TurnosID",
                principalTable: "Turnos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTurno_Personal_PersonalId",
                table: "PersonalTurno");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTurno_Turnos_TurnosID",
                table: "PersonalTurno");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "PersonalId",
                table: "PersonalTurno",
                newName: "PersonalID");

            migrationBuilder.RenameColumn(
                name: "TurnosID",
                table: "PersonalTurno",
                newName: "TurnoID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTurno_TurnosID",
                table: "PersonalTurno",
                newName: "IX_PersonalTurno_TurnoID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Personal",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "Personal",
                newName: "estado");

            migrationBuilder.AlterColumn<int>(
                name: "DNI",
                table: "Personal",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Personal",
                keyColumn: "ID",
                keyValue: 1,
                column: "DNI",
                value: 25123456);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTurno_Personal_PersonalID",
                table: "PersonalTurno",
                column: "PersonalID",
                principalTable: "Personal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTurno_Turnos_TurnoID",
                table: "PersonalTurno",
                column: "TurnoID",
                principalTable: "Turnos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
