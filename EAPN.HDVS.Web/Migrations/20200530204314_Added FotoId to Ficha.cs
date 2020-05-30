using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedFotoIdtoFicha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FotoId",
                schema: "dbo",
                table: "Fichas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas",
                column: "FotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_Adjuntos_FotoId",
                schema: "dbo",
                table: "Fichas",
                column: "FotoId",
                principalSchema: "dbo",
                principalTable: "Adjuntos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_Adjuntos_FotoId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropColumn(
                name: "FotoId",
                schema: "dbo",
                table: "Fichas");
        }
    }
}
