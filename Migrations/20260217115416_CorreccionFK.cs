using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_GESTION_HOGAR.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hijos_Mujeres_MujerID",
                table: "Hijos");

            migrationBuilder.DropForeignKey(
                name: "FK_Medidas_Denuncias_DenunciaID",
                table: "Medidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Registros_Egresos_EgresoID",
                table: "Registros");

            migrationBuilder.DropIndex(
                name: "IX_Registros_EgresoID",
                table: "Registros");

            migrationBuilder.DropColumn(
                name: "EgresoID",
                table: "Registros");

            migrationBuilder.DropColumn(
                name: "IdHabitacion",
                table: "Registros");

            migrationBuilder.DropColumn(
                name: "IdMujer",
                table: "Registros");

            migrationBuilder.DropColumn(
                name: "IdRegistro",
                table: "Observaciones");

            migrationBuilder.DropColumn(
                name: "IdDenuncia",
                table: "Medidas");

            migrationBuilder.DropColumn(
                name: "IDMadre",
                table: "Hijos");

            migrationBuilder.DropColumn(
                name: "IdRegistro",
                table: "Denuncias");

            migrationBuilder.DropColumn(
                name: "IdRegistro",
                table: "Agresores");

            migrationBuilder.RenameColumn(
                name: "DenunciaID",
                table: "Medidas",
                newName: "DenunciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Medidas_DenunciaID",
                table: "Medidas",
                newName: "IX_Medidas_DenunciaId");

            migrationBuilder.RenameColumn(
                name: "MujerID",
                table: "Hijos",
                newName: "MujerId");

            migrationBuilder.RenameIndex(
                name: "IX_Hijos_MujerID",
                table: "Hijos",
                newName: "IX_Hijos_MujerId");

            migrationBuilder.RenameColumn(
                name: "IdRegistro",
                table: "Habitaciones",
                newName: "RegistroId");

            migrationBuilder.RenameColumn(
                name: "Idregistro",
                table: "Egresos",
                newName: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Egresos_RegistroId",
                table: "Egresos",
                column: "RegistroId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Egresos_Registros_RegistroId",
                table: "Egresos",
                column: "RegistroId",
                principalTable: "Registros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hijos_Mujeres_MujerId",
                table: "Hijos",
                column: "MujerId",
                principalTable: "Mujeres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medidas_Denuncias_DenunciaId",
                table: "Medidas",
                column: "DenunciaId",
                principalTable: "Denuncias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Egresos_Registros_RegistroId",
                table: "Egresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Hijos_Mujeres_MujerId",
                table: "Hijos");

            migrationBuilder.DropForeignKey(
                name: "FK_Medidas_Denuncias_DenunciaId",
                table: "Medidas");

            migrationBuilder.DropIndex(
                name: "IX_Egresos_RegistroId",
                table: "Egresos");

            migrationBuilder.RenameColumn(
                name: "DenunciaId",
                table: "Medidas",
                newName: "DenunciaID");

            migrationBuilder.RenameIndex(
                name: "IX_Medidas_DenunciaId",
                table: "Medidas",
                newName: "IX_Medidas_DenunciaID");

            migrationBuilder.RenameColumn(
                name: "MujerId",
                table: "Hijos",
                newName: "MujerID");

            migrationBuilder.RenameIndex(
                name: "IX_Hijos_MujerId",
                table: "Hijos",
                newName: "IX_Hijos_MujerID");

            migrationBuilder.RenameColumn(
                name: "RegistroId",
                table: "Habitaciones",
                newName: "IdRegistro");

            migrationBuilder.RenameColumn(
                name: "RegistroId",
                table: "Egresos",
                newName: "Idregistro");

            migrationBuilder.AddColumn<int>(
                name: "EgresoID",
                table: "Registros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdHabitacion",
                table: "Registros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdMujer",
                table: "Registros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdRegistro",
                table: "Observaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdDenuncia",
                table: "Medidas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IDMadre",
                table: "Hijos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdRegistro",
                table: "Denuncias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdRegistro",
                table: "Agresores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registros_EgresoID",
                table: "Registros",
                column: "EgresoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Hijos_Mujeres_MujerID",
                table: "Hijos",
                column: "MujerID",
                principalTable: "Mujeres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medidas_Denuncias_DenunciaID",
                table: "Medidas",
                column: "DenunciaID",
                principalTable: "Denuncias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registros_Egresos_EgresoID",
                table: "Registros",
                column: "EgresoID",
                principalTable: "Egresos",
                principalColumn: "ID");
        }
    }
}
