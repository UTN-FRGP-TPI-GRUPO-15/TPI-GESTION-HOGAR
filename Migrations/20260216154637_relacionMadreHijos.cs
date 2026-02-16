using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class relacionMadreHijos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mujeres",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NivelEducativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ocupacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dommicilio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locaclidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mujeres", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Hijos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    IDMadre = table.Column<int>(type: "int", nullable: false),
                    MujerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hijos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hijos_mujeres_MujerID",
                        column: x => x.MujerID,
                        principalTable: "mujeres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hijos_MujerID",
                table: "Hijos",
                column: "MujerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hijos");

            migrationBuilder.DropTable(
                name: "mujeres");
        }
    }
}
