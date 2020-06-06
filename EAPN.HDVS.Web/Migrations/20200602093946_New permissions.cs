using EAPN.HDVS.Shared.Permissions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Newpermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_UsuarioId",
                schema: "dbo",
                table: "Seguimientos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Usuarios_UsuarioId",
                schema: "dbo",
                table: "Seguimientos",
                column: "UsuarioId",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Permisos",
                columns: new[] { "Id", "Descripcion", "Clave" },
                values: new object[,]
                {
                    { 11, "Adjuntos: Lectura", Permissions.PERSONALATTACHMENTS_READ},
                    { 12, "Adjuntos: Escritura", Permissions.PERSONALATTACHMENTS_WRITE },
                    { 13, "Adjuntos: Eliminar", Permissions.PERSONALATTACHMENTS_DELETE },
                    { 14, "Adjuntos: Acceder", Permissions.PERSONALATTACHMENTS_ACCESS },
                    { 15, "Seguimientos: Lectura", Permissions.PERSONALINDICATORS_READ},
                    { 16, "Seguimientos: Escritura", Permissions.PERSONALINDICATORS_WRITE },
                    { 17, "Seguimientos: Eliminar", Permissions.PERSONALINDICATORS_DELETE },
                    { 18, "Seguimientos: Acceder", Permissions.PERSONALINDICATORS_ACCESS },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Usuarios_UsuarioId",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_UsuarioId",
                schema: "dbo",
                table: "Seguimientos");
        }
    }
}
