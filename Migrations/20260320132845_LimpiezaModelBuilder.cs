using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class LimpiezaModelBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "Denuncias",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hijos",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personal",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Registros",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mujeres",
                keyColumn: "ID",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mujeres",
                columns: new[] { "ID", "Apellido", "DNI", "Domicilio", "Estado", "FechaNac", "Genero", "Localidad", "Nacionalidad", "NivelEducativo", "Nombre", "Ocupacion", "Provincia", "Telefono" },
                values: new object[] { 1, "López", 30111222, "Av. San Martín 456", true, new DateOnly(1985, 10, 20), "Femenino", "Mar de Ajó", "Argentina", "Secundario Completo", "María", "Empleada de Comercio", null, "2246-554433" });

            migrationBuilder.InsertData(
                table: "Personal",
                columns: new[] { "Id", "Activo", "Apellido", "DNI", "Domicilio", "FechaNac", "Legajo", "Localidad", "Nacionalidad", "Nombre", "Telefono" },
                values: new object[] { 1, true, "García", "25123456", "Calle 4 Nro 123", new DateOnly(1980, 5, 12), 1001, "San Clemente del Tuyú", "Argentina", "Laura", "2246-112233" });

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
                columns: new[] { "Id", "Clave", "Email", "NombreUsuario", "PersonalId", "ResetToken", "ResetTokenExpiry", "RolId" },
                values: new object[] { 1, "123456", "lgarcia@test.com", "lgarcia", 1, null, null, 2 });

            migrationBuilder.InsertData(
                table: "Agresores",
                columns: new[] { "ID", "Apellido", "DNI", "Domicilio", "FechaNac", "Genero", "Localidad", "Nacionalidad", "NivelEducativo", "Nombre", "Ocupacion", "RegistroId", "Telefono", "Vinculo" },
                values: new object[] { 1, "Pérez", 29888777, "Av. San Martín 456", new DateOnly(1982, 8, 5), "Masculino", "Mar de Ajó", "Argentina", "Primario", "Carlos", "Albañil", 1, "2246-998877", "Ex-Pareja" });

            migrationBuilder.InsertData(
                table: "Denuncias",
                columns: new[] { "ID", "Fecha", "NroExp", "NroIPP", "RegistroId" },
                values: new object[] { 1, new DateOnly(2026, 2, 9), 67890, 12345, 1 });
        }
    }
}
