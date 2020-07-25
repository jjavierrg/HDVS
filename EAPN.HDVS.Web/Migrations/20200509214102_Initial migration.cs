using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Paises",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfiles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    Permiso = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sexos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 250, nullable: false),
                    Nombre = table.Column<string>(maxLength: 150, nullable: true),
                    Apellidos = table.Column<string>(maxLength: 150, nullable: true),
                    Hash = table.Column<string>(maxLength: 128, nullable: false),
                    IntentosLogin = table.Column<int>(nullable: false),
                    Activo = table.Column<bool>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    FinBloqueo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilesRoles",
                schema: "dbo",
                columns: table => new
                {
                    PerfilId = table.Column<int>(nullable: false),
                    RolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesRoles", x => new { x.PerfilId, x.RolId });
                    table.ForeignKey(
                        name: "FK_PerfilesRoles_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalSchema: "dbo",
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilesRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    Apellido1 = table.Column<string>(maxLength: 250, nullable: true),
                    Apellido2 = table.Column<string>(maxLength: 250, nullable: true),
                    DNI = table.Column<string>(maxLength: 12, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: true),
                    NacionalidadId = table.Column<int>(nullable: false),
                    NacionalidadId1 = table.Column<int>(nullable: false),
                    DocumentacionEmpadronamiento = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personas_Paises_NacionalidadId1",
                        column: x => x.NacionalidadId1,
                        principalSchema: "dbo",
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personas_Sexos_NacionalidadId",
                        column: x => x.NacionalidadId,
                        principalSchema: "dbo",
                        principalTable: "Sexos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(maxLength: 128, nullable: false),
                    FinValidez = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "dbo",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosPerfiles",
                schema: "dbo",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false),
                    PerfilId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosPerfiles", x => new { x.UsuarioId, x.PerfilId });
                    table.ForeignKey(
                        name: "FK_UsuariosPerfiles_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalSchema: "dbo",
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosPerfiles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "dbo",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                schema: "dbo",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false),
                    RolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "dbo",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Direcciones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false),
                    DireccionCompleta = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Direcciones_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalSchema: "dbo",
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Paises",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Afganistán" },
                    { 126, "Nauru" },
                    { 127, "Nepal" },
                    { 128, "Nicaragua" },
                    { 129, "Níger" },
                    { 130, "Nigeria" },
                    { 131, "Noruega" },
                    { 132, "Nueva Zelanda" },
                    { 133, "Omán" },
                    { 134, "Países Bajos" },
                    { 125, "Namibia" },
                    { 135, "Pakistán" },
                    { 137, "Panamá" },
                    { 138, "Papúa Nueva Guinea" },
                    { 139, "Paraguay" },
                    { 140, "Perú" },
                    { 141, "Polonia" },
                    { 142, "Portugal" },
                    { 143, "Reino Unido de Gran Bretaña e Irlanda del Norte" },
                    { 144, "República Centroafricana" },
                    { 145, "República Checa" },
                    { 136, "Palaos" },
                    { 124, "Mozambique" },
                    { 123, "Montenegro" },
                    { 122, "Mongolia" },
                    { 100, "Lesoto" },
                    { 101, "Letonia" },
                    { 102, "Líbano" },
                    { 103, "Liberia" },
                    { 105, "Liechtenstein" },
                    { 106, "Lituania" },
                    { 107, "Luxemburgo" },
                    { 108, "Macedonia del Norte" },
                    { 109, "Madagascar" },
                    { 110, "Malasia" },
                    { 111, "Malaui" },
                    { 112, "Maldivas" },
                    { 113, "Malí" },
                    { 114, "Malta" },
                    { 115, "Marruecos" },
                    { 116, "Mauricio" },
                    { 117, "Mauritania" },
                    { 118, "México" },
                    { 119, "Micronesia" },
                    { 120, "Moldavia" },
                    { 121, "Mónaco" },
                    { 146, "República del Congo" },
                    { 99, "Laos" },
                    { 147, "República Democrática del Congo" },
                    { 149, "República Sudafricana" },
                    { 175, "Tayikistán" },
                    { 176, "Timor Oriental" },
                    { 177, "Togo" },
                    { 178, "Tonga" },
                    { 179, "Trinidad y Tobago" },
                    { 180, "Túnez" },
                    { 181, "Turkmenistán" },
                    { 182, "Turquía" },
                    { 183, "Tuvalu" },
                    { 174, "Tanzania" },
                    { 184, "Ucrania" },
                    { 186, "Uruguay" },
                    { 187, "Uzbekistán" },
                    { 188, "Vanuatu" },
                    { 189, "Venezuela" },
                    { 190, "Vietnam" },
                    { 191, "Yemen" },
                    { 192, "Yibuti" },
                    { 193, "Zambia" },
                    { 194, "Zimbabue" },
                    { 185, "Uganda" },
                    { 173, "Tailandia" },
                    { 172, "Surinam" },
                    { 171, "Suiza" },
                    { 150, "Ruanda" },
                    { 151, "Rumanía" },
                    { 152, "Rusia" },
                    { 153, "Samoa" },
                    { 154, "San Cristóbal y Nieves" },
                    { 155, "San Marino" },
                    { 156, "San Vicente y las Granadinas" },
                    { 157, "Santa Lucía" },
                    { 158, "Santo Tomé y Príncipe" },
                    { 159, "Senegal" },
                    { 160, "Serbia" },
                    { 161, "Seychelles" },
                    { 162, "Sierra Leona" },
                    { 163, "Singapur" },
                    { 164, "Siria" },
                    { 165, "Somalia" },
                    { 166, "Sri Lanka" },
                    { 167, "Suazilandia" },
                    { 168, "Sudán" },
                    { 169, "Sudán del Sur" },
                    { 170, "Suecia" },
                    { 148, "República Dominicana" },
                    { 98, "Kuwait" },
                    { 104, "Libia" },
                    { 96, "Kirguistán" },
                    { 27, "Brunéi" },
                    { 28, "Bulgaria" },
                    { 29, "Burkina Faso" },
                    { 30, "Burundi" },
                    { 31, "Bután" },
                    { 32, "Cabo Verde" },
                    { 33, "Camboya" },
                    { 34, "Camerún" },
                    { 35, "Canadá" },
                    { 26, "Brasil" },
                    { 36, "Catar" },
                    { 38, "Chile" },
                    { 39, "China" },
                    { 40, "Chipre" },
                    { 41, "Ciudad del Vaticano" },
                    { 42, "Colombia" },
                    { 97, "Kiribati" },
                    { 44, "Corea del Norte" },
                    { 45, "Corea del Sur" },
                    { 46, "Costa de Marfil" },
                    { 37, "Chad" },
                    { 25, "Botsuana" },
                    { 24, "Bosnia y Herzegovina" },
                    { 23, "Bolivia" },
                    { 2, "Albania" },
                    { 3, "Alemania" },
                    { 4, "Andorra" },
                    { 5, "Angola" },
                    { 6, "Antigua y Barbuda" },
                    { 7, "Arabia Saudita" },
                    { 8, "Argelia" },
                    { 9, "Argentina" },
                    { 10, "Armenia" },
                    { 11, "Australia" },
                    { 12, "Austria" },
                    { 13, "Azerbaiyán" },
                    { 14, "Bahamas" },
                    { 15, "Bangladés" },
                    { 16, "Barbados" },
                    { 17, "Baréin" },
                    { 18, "Bélgica" },
                    { 19, "Belice" },
                    { 20, "Benín" },
                    { 21, "Bielorrusia" },
                    { 22, "Birmania" },
                    { 47, "Costa Rica" },
                    { 48, "Croacia" },
                    { 43, "Comoras" },
                    { 50, "Dinamarca" },
                    { 76, "Guinea-Bisáu" },
                    { 77, "Guinea Ecuatorial" },
                    { 78, "Haití" },
                    { 49, "Cuba" },
                    { 80, "Hungría" },
                    { 81, "India" },
                    { 82, "Indonesia" },
                    { 83, "Irak" },
                    { 84, "Irán" },
                    { 75, "Guinea" },
                    { 85, "Irlanda" },
                    { 87, "Islas Marshall" },
                    { 88, "Islas Salomón" },
                    { 89, "Israel" },
                    { 90, "Italia" },
                    { 91, "Jamaica" },
                    { 92, "Japón" },
                    { 93, "Jordania" },
                    { 94, "Kazajistán" },
                    { 95, "Kenia" },
                    { 86, "Islandia" },
                    { 74, "Guyana" },
                    { 79, "Honduras" },
                    { 72, "Grecia" },
                    { 51, "Dominica" },
                    { 52, "Ecuador" },
                    { 53, "Egipto" },
                    { 54, "El Salvador" },
                    { 73, "Guatemala" },
                    { 56, "Eritrea" },
                    { 57, "Eslovaquia" },
                    { 58, "Eslovenia" },
                    { 59, "España" },
                    { 60, "Estados Unidos" },
                    { 55, "Emiratos Árabes Unidos" },
                    { 62, "Etiopía" },
                    { 63, "Filipinas" },
                    { 64, "Finlandia" },
                    { 65, "Fiyi" },
                    { 66, "Francia" },
                    { 67, "Gabón" },
                    { 68, "Gambia" },
                    { 69, "Georgia" },
                    { 70, "Ghana" },
                    { 71, "Granada" },
                    { 61, "Estonia" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Perfiles",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 2, "Administrador" },
                    { 1, "Usuario" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Permiso" },
                values: new object[,]
                {
                    { 1, "Aplicación: Acceder", "user:login" },
                    { 2, "Usuarios: Lectura", "usermng:read" },
                    { 3, "Usuarios: Escritura", "usermng:write" },
                    { 4, "Usuarios: Eliminar", "usermng:delete" },
                    { 5, "Usuarios: Acceder", "usermng:access" },
                    { 6, "Usuarios: Superadministrador", "usermng:admin" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Sexos",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Hombre" },
                    { 2, "Mujer" },
                    { 3, "Otros" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Apellidos", "Email", "FinBloqueo", "Hash", "IntentosLogin", "Nombre", "Observaciones" },
                values: new object[,]
                {
                    { 1, true, "Rodríguez Gallego", "Jjavierrg@gmail.com", null, "$2b$12$sn1hzFuoYJwKxocoCsn3me4gLtEeMG9sJrz/FQQu6XMww3bdoSgOe", 0, "José Javier", null },
                    { 2, true, "Test", "test@test.com", null, "$2b$12$vrDFckbZnoXbB9oFeLqqn.UQtnQ2UdYOJC/r6UqrjLfS00LagnO0q", 0, "Test", null }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PerfilesRoles",
                columns: new[] { "PerfilId", "RolId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "UsuariosPerfiles",
                columns: new[] { "UsuarioId", "PerfilId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 1 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "UsuariosRoles",
                columns: new[] { "UsuarioId", "RolId" },
                values: new object[] { 1, 6 });

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_PersonaId",
                schema: "dbo",
                table: "Direcciones",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesRoles_RolId",
                schema: "dbo",
                table: "PerfilesRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_NacionalidadId1",
                schema: "dbo",
                table: "Personas",
                column: "NacionalidadId1");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_NacionalidadId",
                schema: "dbo",
                table: "Personas",
                column: "NacionalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                schema: "dbo",
                table: "RefreshTokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosPerfiles_PerfilId",
                schema: "dbo",
                table: "UsuariosPerfiles",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_RolId",
                schema: "dbo",
                table: "UsuariosRoles",
                column: "RolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Direcciones",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PerfilesRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UsuariosPerfiles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UsuariosRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Personas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Perfiles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Paises",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Sexos",
                schema: "dbo");
        }
    }
}
