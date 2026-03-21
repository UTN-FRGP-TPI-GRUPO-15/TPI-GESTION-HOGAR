using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProvinviaMujer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Mujeres",
                newName: "Estado");

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Mujeres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Mujeres",
                keyColumn: "ID",
                keyValue: 1,
                column: "Provincia",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Mujeres");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Mujeres",
                newName: "estado");
        }
    }
}
