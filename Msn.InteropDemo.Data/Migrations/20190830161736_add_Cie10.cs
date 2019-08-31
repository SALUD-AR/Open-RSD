using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_Cie10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cie10",
                columns: table => new
                {
                    SubcategoriaId = table.Column<string>(type: "char(4)", unicode: false, maxLength: 4, nullable: false),
                    SubcategoriaNombre = table.Column<string>(maxLength: 300, nullable: false),
                    CategoriaNombre = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cie10", x => x.SubcategoriaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cie10");
        }
    }
}
