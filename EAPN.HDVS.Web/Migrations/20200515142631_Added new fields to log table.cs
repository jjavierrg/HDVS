using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Addednewfieldstologtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                schema: "dbo",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                schema: "dbo",
                table: "Logs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelOrder",
                schema: "dbo",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                schema: "dbo",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "LevelOrder",
                schema: "dbo",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "User",
                schema: "dbo",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
