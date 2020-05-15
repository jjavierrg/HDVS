using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Logtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PerfilesPermisos",
                keyColumns: new[] { "PerfilId", "PermisoId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PerfilesPermisos",
                keyColumns: new[] { "PerfilId", "PermisoId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Logger = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    CallSite = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs",
                schema: "dbo");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Permisos",
                columns: new[] { "Id", "Clave", "Descripcion" },
                values: new object[,]
                {
                    { 1, "user:login", "Aplicación: Acceder" },
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PerfilesPermisos",
                columns: new[] { "PerfilId", "PermisoId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                });
        }
    }
}
