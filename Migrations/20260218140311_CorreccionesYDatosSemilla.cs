using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionesYDatosSemilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "Estado", "Fecha", "FechaEgreso", "HabitacionId", "MujerID" },
                values: new object[] { 1, true, new DateOnly(2026, 2, 10), null, 1, 1 });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agresores",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Condiciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Hijos",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medidas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medidas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ObservacionCondiciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ObservacionCondiciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoCondiciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoCondiciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoCondiciones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TipoCondiciones",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TipoTurnos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoTurnos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoTurnos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Denuncias",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ObservacionCondiciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personal",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoCondiciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Registros",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mujeres",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
