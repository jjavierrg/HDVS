using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedVerificadotoIndicadorSeguimientotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verificado",
                schema: "dbo",
                table: "IndicadoresSeguimientos",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "Sugerencias",
                table: "Indicadores",
                newName: "Verificacion",
                schema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verificado",
                schema: "dbo",
                table: "IndicadoresSeguimientos");

            migrationBuilder.RenameColumn(
                name: "Verificacion",
                table: "Indicadores",
                newName: "Sugerencias",
                schema: "dbo");
        }
    }
}
