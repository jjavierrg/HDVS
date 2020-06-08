using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedRangoentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rangos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    Minimo = table.Column<int>(nullable: false),
                    Maximo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rangos", x => x.Id);
                });

            migrationBuilder.InsertData(
               schema: "dbo",
               table: "Rangos",
               columns: new[] { "Id", "Descripcion", "Minimo", "Maximo" },
               values: new object[,]
               {
                    { 1, "Sin Vulnerabilidad", 0, 9},
                    { 2, "Vulnerabilidad leve", 10, 19},
                    { 3, "Vulnerabilidad moderada", 20, 29},
                    { 4, "Vulnerabilidad grave", 30, 44},
                    { 5, "Vulnerabilidad extrema", 45, null},
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rangos",
                schema: "dbo");
        }
    }
}
