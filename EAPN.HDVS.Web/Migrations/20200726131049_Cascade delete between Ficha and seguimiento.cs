using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class CascadedeletebetweenFichaandseguimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Fichas_FichaId",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Fichas_FichaId",
                schema: "dbo",
                table: "Seguimientos",
                column: "FichaId",
                principalSchema: "dbo",
                principalTable: "Fichas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Fichas_FichaId",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Fichas_FichaId",
                schema: "dbo",
                table: "Seguimientos",
                column: "FichaId",
                principalSchema: "dbo",
                principalTable: "Fichas",
                principalColumn: "Id");
        }
    }
}
