using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedcompletofieldtoSeguimientoentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completo",
                schema: "dbo",
                table: "Seguimientos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completo",
                schema: "dbo",
                table: "Seguimientos");
        }
    }
}
