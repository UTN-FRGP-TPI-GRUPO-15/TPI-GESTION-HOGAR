using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class TablasRegistros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hijos_mujeres_MujerID",
                table: "Hijos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mujeres",
                table: "mujeres");

            migrationBuilder.RenameTable(
                name: "mujeres",
                newName: "Mujeres");

            migrationBuilder.RenameColumn(
                name: "dommicilio",
                table: "Mujeres",
                newName: "Domicilio");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mujeres",
                table: "Mujeres",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Egresos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    ApellidoRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNIRef = table.Column<int>(type: "int", nullable: true),
                    TelefonoRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomicilioRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaclidadRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idregistro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egresos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Habitaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroHabitacion = table.Column<int>(type: "int", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    IdRegistro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaEgreso = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    IdMujer = table.Column<int>(type: "int", nullable: false),
                    IdHabitacion = table.Column<int>(type: "int", nullable: false),
                    MujerID = table.Column<int>(type: "int", nullable: false),
                    HabitacionId = table.Column<int>(type: "int", nullable: false),
                    EgresoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registros_Egresos_EgresoID",
                        column: x => x.EgresoID,
                        principalTable: "Egresos",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Registros_Habitaciones_HabitacionId",
                        column: x => x.HabitacionId,
                        principalTable: "Habitaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registros_Mujeres_MujerID",
                        column: x => x.MujerID,
                        principalTable: "Mujeres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agresores",
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
                    Domicilio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locaclidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdRegistro = table.Column<int>(type: "int", nullable: false),
                    RegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agresores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Agresores_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Denuncias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    NroIPP = table.Column<int>(type: "int", nullable: true),
                    NroExp = table.Column<int>(type: "int", nullable: true),
                    IdRegistro = table.Column<int>(type: "int", nullable: false),
                    RegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Denuncias_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Suceso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRegistro = table.Column<int>(type: "int", nullable: false),
                    RegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observaciones_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDenuncia = table.Column<int>(type: "int", nullable: false),
                    DenunciaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medidas_Denuncias_DenunciaID",
                        column: x => x.DenunciaID,
                        principalTable: "Denuncias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agresores_RegistroId",
                table: "Agresores",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_RegistroId",
                table: "Denuncias",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Medidas_DenunciaID",
                table: "Medidas",
                column: "DenunciaID");

            migrationBuilder.CreateIndex(
                name: "IX_Observaciones_RegistroId",
                table: "Observaciones",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_EgresoID",
                table: "Registros",
                column: "EgresoID");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_HabitacionId",
                table: "Registros",
                column: "HabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_MujerID",
                table: "Registros",
                column: "MujerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hijos_Mujeres_MujerID",
                table: "Hijos",
                column: "MujerID",
                principalTable: "Mujeres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hijos_Mujeres_MujerID",
                table: "Hijos");

            migrationBuilder.DropTable(
                name: "Agresores");

            migrationBuilder.DropTable(
                name: "Medidas");

            migrationBuilder.DropTable(
                name: "Observaciones");

            migrationBuilder.DropTable(
                name: "Denuncias");

            migrationBuilder.DropTable(
                name: "Registros");

            migrationBuilder.DropTable(
                name: "Egresos");

            migrationBuilder.DropTable(
                name: "Habitaciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mujeres",
                table: "Mujeres");

            migrationBuilder.RenameTable(
                name: "Mujeres",
                newName: "mujeres");

            migrationBuilder.RenameColumn(
                name: "Domicilio",
                table: "mujeres",
                newName: "dommicilio");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mujeres",
                table: "mujeres",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hijos_mujeres_MujerID",
                table: "Hijos",
                column: "MujerID",
                principalTable: "mujeres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
