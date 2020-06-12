using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedfotoIdcolumntoorganizacionentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FotoId",
                schema: "dbo",
                table: "Organizaciones",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizaciones_FotoId",
                schema: "dbo",
                table: "Organizaciones",
                column: "FotoId",
                unique: true,
                filter: "[FotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizaciones_Adjuntos_FotoId",
                schema: "dbo",
                table: "Organizaciones",
                column: "FotoId",
                principalSchema: "dbo",
                principalTable: "Adjuntos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizaciones_Adjuntos_FotoId",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.DropIndex(
                name: "IX_Organizaciones_FotoId",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.DropColumn(
                name: "FotoId",
                schema: "dbo",
                table: "Organizaciones");
        }
    }
}
