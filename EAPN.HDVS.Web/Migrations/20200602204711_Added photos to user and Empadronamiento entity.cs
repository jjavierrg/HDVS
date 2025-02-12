using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedphotostouserandEmpadronamientoentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Fichas SET PadronId = NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_Municipios_PadronId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.AddColumn<int>(
                name: "FotoId",
                schema: "dbo",
                table: "Usuarios",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Empadronamientos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empadronamientos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FotoId",
                schema: "dbo",
                table: "Usuarios",
                column: "FotoId",
                unique: true,
                filter: "[FotoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas",
                column: "FotoId",
                unique: true,
                filter: "[FotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_Empadronamientos_PadronId",
                schema: "dbo",
                table: "Fichas",
                column: "PadronId",
                principalSchema: "dbo",
                principalTable: "Empadronamientos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Adjuntos_FotoId",
                schema: "dbo",
                table: "Usuarios",
                column: "FotoId",
                principalSchema: "dbo",
                principalTable: "Adjuntos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Permiso de trabajo y residencia",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Permiso de residencia",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Situación irregular",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripcion",
                value: "Protección internacional",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Descripcion",
                value: "Solicitante de Protección internacional",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 6,
                column: "Descripcion",
                value: "NIE ciudadanos comunitarios",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descripcion",
                value: "DNI",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 8,
                column: "Descripcion",
                value: "Autorización adm. especial penados extranjeros/as",
                schema: "dbo");

            migrationBuilder.UpdateData(
                table: "SituacionesAdministrativas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Descripcion",
                value: "NIE por razones humanitarias",
                schema: "dbo");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Empadronamientos",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Sí" } ,
                    { 2, "No" } ,
                    { 3, "En trámite" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_Empadronamientos_PadronId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Adjuntos_FotoId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Empadronamientos",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_FotoId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropColumn(
                name: "FotoId",
                schema: "dbo",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_FotoId",
                schema: "dbo",
                table: "Fichas",
                column: "FotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_Municipios_PadronId",
                schema: "dbo",
                table: "Fichas",
                column: "PadronId",
                principalSchema: "dbo",
                principalTable: "Municipios",
                principalColumn: "Id");
        }
    }
}
