using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Habitaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroHabitacion = table.Column<int>(type: "int", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mujeres",
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
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mujeres", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ObservacionCondiciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservacionCondiciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Legajo = table.Column<int>(type: "int", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domicilio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCondiciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCondiciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTurnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTurnos", x => x.Id);
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
                    MujerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hijos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hijos_Mujeres_MujerId",
                        column: x => x.MujerId,
                        principalTable: "Mujeres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    MujerID = table.Column<int>(type: "int", nullable: false),
                    HabitacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.Id);
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
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Personal_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Condiciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MujerId = table.Column<int>(type: "int", nullable: false),
                    TipoCondicionId = table.Column<int>(type: "int", nullable: false),
                    ObservacionCondicionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condiciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condiciones_Mujeres_MujerId",
                        column: x => x.MujerId,
                        principalTable: "Mujeres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Condiciones_ObservacionCondiciones_ObservacionCondicionId",
                        column: x => x.ObservacionCondicionId,
                        principalTable: "ObservacionCondiciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Condiciones_TipoCondiciones_TipoCondicionId",
                        column: x => x.TipoCondicionId,
                        principalTable: "TipoCondiciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Horas = table.Column<int>(type: "int", nullable: false),
                    TipoTurnoId = table.Column<int>(type: "int", nullable: false),
                    PersonalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Turnos_TipoTurnos_TipoTurnoId",
                        column: x => x.TipoTurnoId,
                        principalTable: "TipoTurnos",
                        principalColumn: "Id",
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
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    LocalidadRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egresos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Egresos_Registros_RegistroId",
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

            migrationBuilder.CreateTable(
                name: "Medidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DenunciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medidas_Denuncias_DenunciaId",
                        column: x => x.DenunciaId,
                        principalTable: "Denuncias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Habitaciones",
                columns: new[] { "Id", "Capacidad", "Estado", "NroHabitacion" },
                values: new object[,]
                {
                    { 1, 4, true, 1 },
                    { 2, 5, true, 2 },
                    { 3, 6, true, 3 },
                    { 4, 2, true, 4 }
                });

            migrationBuilder.InsertData(
                table: "Mujeres",
                columns: new[] { "ID", "Apellido", "DNI", "Domicilio", "FechaNac", "Genero", "Localidad", "Nacionalidad", "NivelEducativo", "Nombre", "Ocupacion", "Telefono", "estado" },
                values: new object[] { 1, "López", 30111222, "Av. San Martín 456", new DateOnly(1985, 10, 20), "Femenino", "Mar de Ajó", "Argentina", "Secundario Completo", "María", "Empleada de Comercio", "2246-554433", true });

            migrationBuilder.InsertData(
                table: "ObservacionCondiciones",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "En Tratamiento Médico" },
                    { 2, "Sin Tratamiento Actual" },
                    { 3, "Requiere Atención/Derivación" }
                });

            migrationBuilder.InsertData(
                table: "Personal",
                columns: new[] { "ID", "Apellido", "DNI", "Domicilio", "FechaNac", "Legajo", "Localidad", "Nacionalidad", "Nombre", "Telefono", "estado" },
                values: new object[] { 1, "García", 25123456, "Calle 4 Nro 123", new DateOnly(1980, 5, 12), 1001, "San Clemente del Tuyú", "Argentina", "Laura", "2246-112233", true });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Administradora" },
                    { 2, "Equipo Técnico" },
                    { 3, "Operadora" }
                });

            migrationBuilder.InsertData(
                table: "TipoCondiciones",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Enfermedad Crónica" },
                    { 2, "Discapacidad Física" },
                    { 3, "Discapacidad Intelectual/Mental" },
                    { 4, "Adicción (Consumo Problemático)" },
                    { 5, "Otra Condición Médica" }
                });

            migrationBuilder.InsertData(
                table: "TipoTurnos",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Mañana (08:00 a 14:00)" },
                    { 2, "Tarde (14:00 a 20:00)" },
                    { 3, "Noche (20:00 a 08:00)" }
                });

            migrationBuilder.InsertData(
                table: "Condiciones",
                columns: new[] { "Id", "MujerId", "ObservacionCondicionId", "TipoCondicionId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Hijos",
                columns: new[] { "ID", "Apellido", "DNI", "FechaNac", "MujerId", "Nombre" },
                values: new object[] { 1, "Pérez López", 50333444, new DateOnly(2015, 3, 15), 1, "Mateo" });

            migrationBuilder.InsertData(
                table: "Registros",
                columns: new[] { "Id", "Estado", "Fecha", "HabitacionId", "MujerID" },
                values: new object[] { 1, true, new DateOnly(2026, 2, 10), 1, 1 });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Clave", "NombreUsuario", "PersonalId", "RolId" },
                values: new object[] { 1, "123456", "lgarcia", 1, 2 });

            migrationBuilder.InsertData(
                table: "Agresores",
                columns: new[] { "ID", "Apellido", "DNI", "Domicilio", "FechaNac", "Genero", "Localidad", "Nacionalidad", "NivelEducativo", "Nombre", "Ocupacion", "RegistroId", "Telefono" },
                values: new object[] { 1, "Pérez", 29888777, "Av. San Martín 456", new DateOnly(1982, 8, 5), "Masculino", "Mar de Ajó", "Argentina", "Primario", "Carlos", "Albañil", 1, "2246-998877" });

            migrationBuilder.InsertData(
                table: "Denuncias",
                columns: new[] { "ID", "Fecha", "NroExp", "NroIPP", "RegistroId" },
                values: new object[] { 1, new DateOnly(2026, 2, 9), 67890, 12345, 1 });

            migrationBuilder.InsertData(
                table: "Medidas",
                columns: new[] { "Id", "DenunciaId", "Descripcion" },
                values: new object[,]
                {
                    { 1, 1, "Perimetral 300 metros" },
                    { 2, 1, "Entrega de botón antipánico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agresores_RegistroId",
                table: "Agresores",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Condiciones_MujerId",
                table: "Condiciones",
                column: "MujerId");

            migrationBuilder.CreateIndex(
                name: "IX_Condiciones_ObservacionCondicionId",
                table: "Condiciones",
                column: "ObservacionCondicionId");

            migrationBuilder.CreateIndex(
                name: "IX_Condiciones_TipoCondicionId",
                table: "Condiciones",
                column: "TipoCondicionId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_RegistroId",
                table: "Denuncias",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Egresos_RegistroId",
                table: "Egresos",
                column: "RegistroId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hijos_MujerId",
                table: "Hijos",
                column: "MujerId");

            migrationBuilder.CreateIndex(
                name: "IX_Medidas_DenunciaId",
                table: "Medidas",
                column: "DenunciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Observaciones_RegistroId",
                table: "Observaciones",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTurno_TurnoID",
                table: "PersonalTurno",
                column: "TurnoID");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_HabitacionId",
                table: "Registros",
                column: "HabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_MujerID",
                table: "Registros",
                column: "MujerID");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_TipoTurnoId",
                table: "Turnos",
                column: "TipoTurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PersonalId",
                table: "Usuarios",
                column: "PersonalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agresores");

            migrationBuilder.DropTable(
                name: "Condiciones");

            migrationBuilder.DropTable(
                name: "Egresos");

            migrationBuilder.DropTable(
                name: "Hijos");

            migrationBuilder.DropTable(
                name: "Medidas");

            migrationBuilder.DropTable(
                name: "Observaciones");

            migrationBuilder.DropTable(
                name: "PersonalTurno");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "ObservacionCondiciones");

            migrationBuilder.DropTable(
                name: "TipoCondiciones");

            migrationBuilder.DropTable(
                name: "Denuncias");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Registros");

            migrationBuilder.DropTable(
                name: "TipoTurnos");

            migrationBuilder.DropTable(
                name: "Habitaciones");

            migrationBuilder.DropTable(
                name: "Mujeres");
        }
    }
}
