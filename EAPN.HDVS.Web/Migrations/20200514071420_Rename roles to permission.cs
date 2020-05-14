using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Renamerolestopermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerfilesRoles_RolId",
                schema: "dbo",
                table: "PerfilesRoles");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosRoles_RolId",
                schema: "dbo",
                table: "UsuariosRoles");

            migrationBuilder.RenameTable(
                name: "PerfilesRoles",
                schema: "dbo",
                newName: "PerfilesPermisos",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UsuariosRoles",
                schema: "dbo",
                newName: "UsuariosPermisos",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "dbo",
                newName: "Permisos",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "Permiso",
                table: "Permisos",
                schema: "dbo",
                newName: "Clave");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "PerfilesPermisos",
                schema: "dbo",
                newName: "PermisoId");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "UsuariosPermisos",
                schema: "dbo",
                newName: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesPermisos_PermisoId",
                schema: "dbo",
                table: "PerfilesPermisos",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosPermisos_PermisoId",
                schema: "dbo",
                table: "UsuariosPermisos",
                column: "PermisoId");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Clave", "Descripcion" },
                values: new object[] { "user:superadmin", "Aplicación: Superadministrador" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerfilesPermisos_PermisoId",
                schema: "dbo",
                table: "PerfilesPermisos");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosPermisos_PermisoId",
                schema: "dbo",
                table: "UsuariosPermisos");

            migrationBuilder.RenameTable(
                name: "PerfilesPermisos",
                schema: "dbo",
                newName: "PerfilesRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UsuariosPermisos",
                schema: "dbo",
                newName: "UsuariosRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Permisos",
                schema: "dbo",
                newName: "Roles",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "Clave",
                table: "Roles",
                schema: "dbo",
                newName: "Permiso");

            migrationBuilder.RenameColumn(
                name: "PermisoId",
                table: "PerfilesRoles",
                schema: "dbo",
                newName: "RolId");

            migrationBuilder.RenameColumn(
                name: "PermisoId",
                table: "UsuariosRoles",
                schema: "dbo",
                newName: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesRoles_RolId",
                schema: "dbo",
                table: "PerfilesRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_RolId",
                schema: "dbo",
                table: "UsuariosRoles",
                column: "RolId");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Permisos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Clave", "Descripcion" },
                values: new object[] { "usermng:admin", "Usuarios: Superadministrador" });
        }
    }
}
