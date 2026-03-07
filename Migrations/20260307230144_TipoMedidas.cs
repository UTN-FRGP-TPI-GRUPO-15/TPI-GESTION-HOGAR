using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class TipoMedidas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medidas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medidas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Medidas");

            migrationBuilder.AddColumn<int>(
                name: "TipoMedidaId",
                table: "Medidas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposMedidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMedidas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TiposMedidas",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Prohibición de acercamiento (Perimetral)" },
                    { 2, "Exclusión del hogar del agresor" },
                    { 3, "Entrega de Botón Antipánico / Aplicación Móvil" },
                    { 4, "Cese de actos de perturbación / hostigamiento" },
                    { 5, "Restitución de efectos personales" },
                    { 6, "Ronda policial periódica en el domicilio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medidas_TipoMedidaId",
                table: "Medidas",
                column: "TipoMedidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medidas_TiposMedidas_TipoMedidaId",
                table: "Medidas",
                column: "TipoMedidaId",
                principalTable: "TiposMedidas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medidas_TiposMedidas_TipoMedidaId",
                table: "Medidas");

            migrationBuilder.DropTable(
                name: "TiposMedidas");

            migrationBuilder.DropIndex(
                name: "IX_Medidas_TipoMedidaId",
                table: "Medidas");

            migrationBuilder.DropColumn(
                name: "TipoMedidaId",
                table: "Medidas");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Medidas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Medidas",
                columns: new[] { "Id", "DenunciaId", "Descripcion" },
                values: new object[,]
                {
                    { 1, 1, "Perimetral 300 metros" },
                    { 2, 1, "Entrega de botón antipánico" }
                });
        }
    }
}
