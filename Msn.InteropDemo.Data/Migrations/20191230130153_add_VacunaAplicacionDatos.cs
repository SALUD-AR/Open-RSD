using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_VacunaAplicacionDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AplicacionNomivacEsquemaId",
                table: "EvolucionVacunaAplicacion",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AplicacionNomivacVacunaId",
                table: "EvolucionVacunaAplicacion",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAplicacion",
                table: "EvolucionVacunaAplicacion",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "EvolucionVacunaAplicacion",
                unicode: false,
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AplicacionNomivacEsquemaId",
                table: "EvolucionVacunaAplicacion");

            migrationBuilder.DropColumn(
                name: "AplicacionNomivacVacunaId",
                table: "EvolucionVacunaAplicacion");

            migrationBuilder.DropColumn(
                name: "FechaAplicacion",
                table: "EvolucionVacunaAplicacion");

            migrationBuilder.DropColumn(
                name: "Lote",
                table: "EvolucionVacunaAplicacion");
        }
    }
}
