using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_NomivacEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NomivacEsquema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: false),
                    AplicacionDiaMin = table.Column<int>(nullable: true),
                    AplicacionDiaMax = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NomivacEsquema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NomivacVacuna",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: false),
                    SctId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SctTerm = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NomivacVacuna", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "sqlite_sequence",
            //    columns: table => new
            //    {
            //        name = table.Column<string>(maxLength: 100, nullable: false),
            //        seq = table.Column<string>(maxLength: 100, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sqlite_sequence", x => x.name);
            //    });

            migrationBuilder.CreateTable(
                name: "NomivacVacunaEsquema",
                columns: table => new
                {
                    NomivacVacunaId = table.Column<int>(nullable: false),
                    NomivacEsquemaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NomivacVacunaEsquema", x => new { x.NomivacEsquemaId, x.NomivacVacunaId });
                    table.ForeignKey(
                        name: "FK_NomivacVacunaEsquema_NomivacEsquema_NomivacEsquemaId",
                        column: x => x.NomivacEsquemaId,
                        principalTable: "NomivacEsquema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NomivacVacunaEsquema_NomivacVacuna_NomivacVacunaId",
                        column: x => x.NomivacVacunaId,
                        principalTable: "NomivacVacuna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NomivacVacuna_SctId",
                table: "NomivacVacuna",
                column: "SctId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NomivacVacunaEsquema_NomivacVacunaId",
                table: "NomivacVacunaEsquema",
                column: "NomivacVacunaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NomivacVacunaEsquema");

            //migrationBuilder.DropTable(
            //    name: "sqlite_sequence");

            migrationBuilder.DropTable(
                name: "NomivacEsquema");

            migrationBuilder.DropTable(
                name: "NomivacVacuna");
        }
    }
}
