using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Addedcontactinformationtoorganizacionesandusuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                schema: "dbo",
                table: "Usuarios",
                nullable: false,
                defaultValue: DateTime.MinValue);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                schema: "dbo",
                table: "Usuarios",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Organizaciones",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                schema: "dbo",
                table: "Organizaciones",
                nullable: false,
                defaultValue: DateTime.MinValue);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                schema: "dbo",
                table: "Organizaciones",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Web",
                schema: "dbo",
                table: "Organizaciones",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaAlta",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Telefono",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.DropColumn(
                name: "Telefono",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.DropColumn(
                name: "Web",
                schema: "dbo",
                table: "Organizaciones");
        }
    }
}
