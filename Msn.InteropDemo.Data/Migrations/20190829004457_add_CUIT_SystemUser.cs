using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_CUIT_SystemUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CUIT",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CUIT",
                table: "AspNetUsers");
        }
    }
}
