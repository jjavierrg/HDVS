using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedobligatoriofieldtocategoriaandForzadoIncompletotoseguimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForzadoIncompleto",
                schema: "dbo",
                table: "Seguimientos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Obligatorio",
                schema: "dbo",
                table: "Categorias",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForzadoIncompleto",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "Obligatorio",
                schema: "dbo",
                table: "Categorias");
        }
    }
}
