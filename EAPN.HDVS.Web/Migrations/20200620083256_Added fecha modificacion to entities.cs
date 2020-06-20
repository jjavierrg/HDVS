using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Addedfechamodificaciontoentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                schema: "dbo",
                table: "Seguimientos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimaModificacion",
                schema: "dbo",
                table: "Seguimientos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimaModificacion",
                schema: "dbo",
                table: "Fichas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaAlta",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "FechaUltimaModificacion",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "FechaUltimaModificacion",
                schema: "dbo",
                table: "Fichas");
        }
    }
}
