using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedAsociacionentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsociacionId",
                schema: "dbo",
                table: "Usuarios",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoAcceso",
                schema: "dbo",
                table: "Usuarios",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Asociaciones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    Activa = table.Column<bool>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asociaciones", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Asociaciones",
                columns: new[] { "Id", "Activa", "Nombre", "Observaciones" },
                values: new object[] { 1, true, "EAPN Madrid", null });

            migrationBuilder.Sql("UPDATE Usuarios SET AsociacionId = 1 WHERE AsociacionId IS NULL OR AsociacionId = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AsociacionId",
                schema: "dbo",
                table: "Usuarios",
                column: "AsociacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Asociaciones_AsociacionId",
                schema: "dbo",
                table: "Usuarios",
                column: "AsociacionId",
                principalSchema: "dbo",
                principalTable: "Asociaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Asociaciones_AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Asociaciones",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimoAcceso",
                schema: "dbo",
                table: "Usuarios");
        }
    }
}
