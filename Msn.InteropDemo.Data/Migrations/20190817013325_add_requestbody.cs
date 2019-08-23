using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_requestbody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityRequestBody",
                table: "ActivityLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityResponseBody",
                table: "ActivityLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityRequestBody",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "ActivityResponseBody",
                table: "ActivityLog");
        }
    }
}
