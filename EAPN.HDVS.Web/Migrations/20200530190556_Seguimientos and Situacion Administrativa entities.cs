using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class SeguimientosandSituacionAdministrativaentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadorFichas",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "Seguimientos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(nullable: false),
                    FichaId = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Fichas_FichaId",
                        column: x => x.FichaId,
                        principalSchema: "dbo",
                        principalTable: "Fichas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SituacionesAdministrativas",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacionesAdministrativas", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SituacionesAdministrativas",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Empresario con asalariadas/os" } ,
                    { 2, "Autónomo" } ,
                    { 3, "Cooperativista" } ,
                    { 4, "Asalariado" } ,
                    { 5, "En paro" } ,
                    { 6, "Trabajo a tiempo total" } ,
                    { 7, "Trabajo a tiempo parcial" }
                });

            migrationBuilder.Sql("UPDATE Fichas SET SituacionAdministrativaId = 5");

            migrationBuilder.CreateTable(
                name: "IndicadoresSeguimientos",
                schema: "dbo",
                columns: table => new
                {
                    IndicadorId = table.Column<int>(nullable: false),
                    SeguimientoId = table.Column<int>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresSeguimientos", x => new { x.SeguimientoId, x.IndicadorId });
                    table.ForeignKey(
                        name: "FK_IndicadoresSeguimientos_Indicadores_IndicadorId",
                        column: x => x.IndicadorId,
                        principalSchema: "dbo",
                        principalTable: "Indicadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicadoresSeguimientos_Seguimientos_SeguimientoId",
                        column: x => x.SeguimientoId,
                        principalSchema: "dbo",
                        principalTable: "Seguimientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fichas_SituacionAdministrativaId",
                schema: "dbo",
                table: "Fichas",
                column: "SituacionAdministrativaId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresSeguimientos_IndicadorId",
                schema: "dbo",
                table: "IndicadoresSeguimientos",
                column: "IndicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_FichaId",
                schema: "dbo",
                table: "Seguimientos",
                column: "FichaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fichas_SituacionesAdministrativas_SituacionAdministrativaId",
                schema: "dbo",
                table: "Fichas",
                column: "SituacionAdministrativaId",
                principalSchema: "dbo",
                principalTable: "SituacionesAdministrativas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fichas_SituacionesAdministrativas_SituacionAdministrativaId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.DropTable(
                name: "IndicadoresSeguimientos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SituacionesAdministrativas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Seguimientos",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Fichas_SituacionAdministrativaId",
                schema: "dbo",
                table: "Fichas");

            migrationBuilder.CreateTable(
                name: "IndicadorFichas",
                schema: "dbo",
                columns: table => new
                {
                    FichaId = table.Column<int>(type: "int", nullable: false),
                    IndicadorId = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadorFichas", x => new { x.FichaId, x.IndicadorId });
                    table.ForeignKey(
                        name: "FK_IndicadorFichas_Fichas_FichaId",
                        column: x => x.FichaId,
                        principalSchema: "dbo",
                        principalTable: "Fichas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IndicadorFichas_Indicadores_IndicadorId",
                        column: x => x.IndicadorId,
                        principalSchema: "dbo",
                        principalTable: "Indicadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorFichas_IndicadorId",
                schema: "dbo",
                table: "IndicadorFichas",
                column: "IndicadorId");
        }
    }
}
