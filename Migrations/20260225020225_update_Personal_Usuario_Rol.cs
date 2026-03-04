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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuarios");

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
        }
    }
}
