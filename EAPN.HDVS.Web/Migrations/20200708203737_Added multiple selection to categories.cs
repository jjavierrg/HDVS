using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Addedmultipleselectiontocategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForzadoIncompleto",
                schema: "dbo",
                table: "Seguimientos");

            migrationBuilder.AddColumn<bool>(
                name: "RespuestaMultiple",
                schema: "dbo",
                table: "Categorias",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 3, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 12, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 14, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 16, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 31, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 43, column: "RespuestaMultiple", value: true, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 42, column: "Orden", value: 8, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 43, column: "Orden", value: 9, schema: "dbo");
            migrationBuilder.UpdateData(table: "Categorias", keyColumn: "Id", keyValue: 44, column: "Orden", value: 10, schema: "dbo");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Categorias",
                columns: new[] { "Id", "Orden", "DimensionId", "Descripcion", "Activo", "Obligatorio", "RespuestaMultiple" },
                values: new object[,]
                {
                    { 45, 7, 9, "Responsabilidades familiares (cont.)", true, false, false },
                    { 46, 11, 9, "Apoyo social y/o familiar (cont.)", true, false, false }
                });

            migrationBuilder.UpdateData(table: "Indicadores", keyColumn: "Id", keyValue: 150, column: "CategoriaId", value: 45, schema: "dbo");
            migrationBuilder.UpdateData(table: "Indicadores", keyColumn: "Id", keyValue: 150, column: "Descripcion", value: "1. Estar a cargo de mayores y/o dependientes", schema: "dbo");
            migrationBuilder.UpdateData(table: "Indicadores", keyColumn: "Id", keyValue: 159, column: "CategoriaId", value: 46, schema: "dbo");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespuestaMultiple",
                schema: "dbo",
                table: "Categorias");

            migrationBuilder.AddColumn<bool>(
                name: "ForzadoIncompleto",
                schema: "dbo",
                table: "Seguimientos",
                type: "bit",
                nullable: true);
        }
    }
}
