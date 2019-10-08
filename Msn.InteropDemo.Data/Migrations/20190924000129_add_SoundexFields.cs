using Microsoft.EntityFrameworkCore.Migrations;

namespace Msn.InteropDemo.Data.Migrations
{
    public partial class add_SoundexFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimerApellidoSoundex",
                table: "Paciente",
                unicode: false,
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimerNombreSoundex",
                table: "Paciente",
                unicode: false,
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimerApellidoSoundex",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "PrimerNombreSoundex",
                table: "Paciente");
        }
    }
}
