using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class Evolucion_Diagnostico_add_SubCatNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cie10SubcategoriaNombre",
                table: "EvolucionDiagnostico",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cie10SubcategoriaNombre",
                table: "EvolucionDiagnostico");
        }
    }
}
