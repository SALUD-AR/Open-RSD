using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_nomivacImmunizationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NomivanImmunizationId",
                table: "EvolucionVacunaAplicacion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomivanImmunizationId",
                table: "EvolucionVacunaAplicacion");
        }
    }
}
