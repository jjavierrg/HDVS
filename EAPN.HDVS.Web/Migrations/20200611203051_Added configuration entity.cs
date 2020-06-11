using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Addedconfigurationentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuraciones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MostrarEnlaces = table.Column<bool>(nullable: false),
                    Enlaces = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Configuraciones",
                columns: new[] { "Id", "MostrarEnlaces", "Enlaces" },
                values: new object[] { 1, true, @"Programa Incorpora|https://intranet.incorpora.org/Incorpora/;", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuraciones",
                schema: "dbo");
        }
    }
}
