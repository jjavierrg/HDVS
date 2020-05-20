using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class ChangeAsociacionentitytoOrganizaciónentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_Asociaciones_AsociacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Asociaciones_AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Asociaciones",
                schema: "dbo",
                newName: "Organizaciones");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_AsociacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.RenameIndex(
                name: "PK_Asociaciones",
                newName: "PK_Organizaciones",
                schema: "dbo",
                table: "Organizaciones");

            migrationBuilder.RenameColumn(
                name: "AsociacionId",
                newName: "OrganizacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "AsociacionId",
                newName: "OrganizacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacionId",
                schema: "dbo",
                table: "Usuarios",
                column: "OrganizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_OrganizacionId",
                schema: "dbo",
                table: "Fichas",
                column: "OrganizacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_Organizaciones_OrganizacionId",
                schema: "dbo",
                table: "Fichas",
                column: "OrganizacionId",
                principalSchema: "dbo",
                principalTable: "Organizaciones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Organizaciones_OrganizacionId",
                schema: "dbo",
                table: "Usuarios",
                column: "OrganizacionId",
                principalSchema: "dbo",
                principalTable: "Organizaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_Organizaciones_OrganizacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Organizaciones_OrganizacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Organizaciones",
                schema: "dbo",
                newName: "Asociaciones");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_OrganizacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_OrganizacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.RenameColumn(
                name: "OrganizacionId",
                newName: "AsociacionId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "OrganizacionId",
                newName: "AsociacionId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.RenameIndex(
                name: "PK_Organizaciones",
                newName: "PK_Asociaciones",
                schema: "dbo",
                table: "Asociaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AsociacionId",
                schema: "dbo",
                table: "Usuarios",
                column: "AsociacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_AsociacionId",
                schema: "dbo",
                table: "Fichas",
                column: "AsociacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_Asociaciones_AsociacionId",
                schema: "dbo",
                table: "Fichas",
                column: "AsociacionId",
                principalSchema: "dbo",
                principalTable: "Asociaciones",
                principalColumn: "Id");

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
    }
}
