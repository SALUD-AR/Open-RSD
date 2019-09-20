using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_DiagnosticoCie10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvolucionDiagnosticoCie10",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvolucionDiagnosticoId = table.Column<int>(nullable: false),
                    Cie10SubcategoriaId = table.Column<string>(type: "char(4)", unicode: false, maxLength: 4, nullable: true),
                    Cie10SubcategoriaNombre = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucionDiagnosticoCie10", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucionDiagnosticoCie10_EvolucionDiagnostico_EvolucionDiagnosticoId",
                        column: x => x.EvolucionDiagnosticoId,
                        principalTable: "EvolucionDiagnostico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvolucionDiagnosticoCie10_EvolucionDiagnosticoId",
                table: "EvolucionDiagnosticoCie10",
                column: "EvolucionDiagnosticoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvolucionDiagnosticoCie10");
        }
    }
}
