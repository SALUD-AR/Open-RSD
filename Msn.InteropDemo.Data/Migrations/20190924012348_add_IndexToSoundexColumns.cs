using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_IndexToSoundexColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Paciente_PrimerApellidoSoundex",
                table: "Paciente",
                column: "PrimerApellidoSoundex");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_PrimerNombreSoundex",
                table: "Paciente",
                column: "PrimerNombreSoundex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paciente_PrimerApellidoSoundex",
                table: "Paciente");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_PrimerNombreSoundex",
                table: "Paciente");
        }
    }
}
