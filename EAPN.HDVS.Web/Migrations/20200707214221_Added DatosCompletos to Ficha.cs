using EAPN.HDVS.Shared.Permissions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedDatosCompletostoFicha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DatosCompletos",
                schema: "dbo",
                table: "Fichas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Permisos",
                columns: new[] { "Id", "Descripcion", "Clave" },
                values: new object[,]
                {
                    { 19, "Estadísticas: Acceso", Permissions.STATS_ACCESS},
                    { 20, "Estadísticas: Datos Globales", Permissions.STATS_GLOBAL },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatosCompletos",
                schema: "dbo",
                table: "Fichas");
        }
    }
}
