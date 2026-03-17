using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class Seguimientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seguimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistroId = table.Column<int>(type: "int", nullable: false),
                    PersonalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Personal_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Capacidad",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 3,
                column: "Capacidad",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 4,
                column: "Capacidad",
                value: 5);

            migrationBuilder.InsertData(
                table: "Habitaciones",
                columns: new[] { "Id", "Capacidad", "Estado", "NroHabitacion" },
                values: new object[,]
                {
                    { 5, 5, true, 5 },
                    { 6, 5, true, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_PersonalId",
                table: "Seguimientos",
                column: "PersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_RegistroId",
                table: "Seguimientos",
                column: "RegistroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seguimientos");

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Capacidad",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 3,
                column: "Capacidad",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 4,
                column: "Capacidad",
                value: 2);
        }
    }
}
