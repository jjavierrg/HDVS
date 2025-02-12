using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EAPN.HDVS.Web.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "dbo");

			migrationBuilder.CreateTable(
				name: "Configuraciones",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					MostrarEnlaces = table.Column<bool>(nullable: false),
					Enlaces = table.Column<string>(maxLength: 1000, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Configuraciones", x => x.Id);
				});

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

			migrationBuilder.CreateTable(
				name: "Paises",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
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
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Perfiles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Permisos",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false),
					Clave = table.Column<string>(maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Permisos", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Provincias",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Nombre = table.Column<string>(maxLength: 40, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Provincias", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Rangos",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false),
					Minimo = table.Column<int>(nullable: false),
					Maximo = table.Column<int>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Rangos", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Sexos",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Sexos", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "SituacionesAdministrativas",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SituacionesAdministrativas", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "TiposAdjunto",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Descripcion = table.Column<string>(maxLength: 50, nullable: false),
					Carpeta = table.Column<string>(maxLength: 255, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TiposAdjunto", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "PerfilesPermisos",
				schema: "dbo",
				columns: table => new
				{
					PerfilId = table.Column<int>(nullable: false),
					PermisoId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PerfilesPermisos", x => new { x.PerfilId, x.PermisoId });
					table.ForeignKey(
						name: "FK_PerfilesPermisos_Perfiles_PerfilId",
						column: x => x.PerfilId,
						principalSchema: "dbo",
						principalTable: "Perfiles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_PerfilesPermisos_Permisos_PermisoId",
						column: x => x.PermisoId,
						principalSchema: "dbo",
						principalTable: "Permisos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Municipios",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					ProvinciaId = table.Column<int>(nullable: false),
					Nombre = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Municipios", x => x.Id);
					table.ForeignKey(
						name: "FK_Municipios_Provincias_ProvinciaId",
						column: x => x.ProvinciaId,
						principalSchema: "dbo",
						principalTable: "Provincias",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Dimensiones",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Orden = table.Column<int>(nullable: false),
					IconoId = table.Column<int>(nullable: true),
					Descripcion = table.Column<string>(maxLength: 150, nullable: false),
					Activo = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Dimensiones", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Categorias",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Orden = table.Column<int>(nullable: false),
					DimensionId = table.Column<int>(nullable: false),
					Descripcion = table.Column<string>(maxLength: 150, nullable: false),
					RespuestaMultiple = table.Column<bool>(nullable: false),
					Obligatorio = table.Column<bool>(nullable: false),
					Activo = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categorias", x => x.Id);
					table.ForeignKey(
						name: "FK_Categorias_Dimensiones_DimensionId",
						column: x => x.DimensionId,
						principalSchema: "dbo",
						principalTable: "Dimensiones",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Indicadores",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Orden = table.Column<int>(nullable: false),
					CategoriaId = table.Column<int>(nullable: false),
					Descripcion = table.Column<string>(nullable: false),
					Activo = table.Column<bool>(nullable: false),
					Puntuacion = table.Column<int>(nullable: false),
					Verificacion = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Indicadores", x => x.Id);
					table.ForeignKey(
						name: "FK_Indicadores_Categorias_CategoriaId",
						column: x => x.CategoriaId,
						principalSchema: "dbo",
						principalTable: "Categorias",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Fichas",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					FotoId = table.Column<int>(nullable: true),
					OrganizacionId = table.Column<int>(nullable: false),
					UsuarioId = table.Column<int>(nullable: false),
					Codigo = table.Column<string>(maxLength: 50, nullable: false),
					Nombre = table.Column<string>(maxLength: 150, nullable: false),
					Apellido1 = table.Column<string>(maxLength: 250, nullable: true),
					Apellido2 = table.Column<string>(maxLength: 250, nullable: true),
					DNI = table.Column<string>(maxLength: 12, nullable: true),
					FotocopiaDNI = table.Column<bool>(nullable: false),
					FechaNacimiento = table.Column<DateTime>(nullable: true),
					SexoId = table.Column<int>(nullable: true),
					GeneroId = table.Column<int>(nullable: true),
					Domicilio = table.Column<string>(maxLength: 250, nullable: true),
					CP = table.Column<string>(maxLength: 10, nullable: true),
					MunicipioId = table.Column<int>(nullable: true),
					ProvinciaId = table.Column<int>(nullable: true),
					PadronId = table.Column<int>(nullable: true),
					DocumentacionEmpadronamiento = table.Column<bool>(nullable: false),
					NacionalidadId = table.Column<int>(nullable: true),
					OrigenId = table.Column<int>(nullable: true),
					SituacionAdministrativaId = table.Column<int>(nullable: true),
					Telefono = table.Column<string>(maxLength: 20, nullable: true),
					Email = table.Column<string>(maxLength: 255, nullable: true),
					MotivoAlta = table.Column<string>(nullable: true),
					Observaciones = table.Column<string>(nullable: true),
					PoliticaFirmada = table.Column<bool>(nullable: false),
					Completa = table.Column<bool>(nullable: false),
					DatosCompletos = table.Column<bool>(nullable: false),
					FechaAlta = table.Column<DateTime>(nullable: false),
					FechaUltimaModificacion = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Fichas", x => x.Id);
					table.ForeignKey(
						name: "FK_Fichas_Sexos_GeneroId",
						column: x => x.GeneroId,
						principalSchema: "dbo",
						principalTable: "Sexos",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Municipios_MunicipioId",
						column: x => x.MunicipioId,
						principalSchema: "dbo",
						principalTable: "Municipios",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Paises_NacionalidadId",
						column: x => x.NacionalidadId,
						principalSchema: "dbo",
						principalTable: "Paises",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Paises_OrigenId",
						column: x => x.OrigenId,
						principalSchema: "dbo",
						principalTable: "Paises",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Empadronamientos_PadronId",
						column: x => x.PadronId,
						principalSchema: "dbo",
						principalTable: "Empadronamientos",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Provincias_ProvinciaId",
						column: x => x.ProvinciaId,
						principalSchema: "dbo",
						principalTable: "Provincias",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_Sexos_SexoId",
						column: x => x.SexoId,
						principalSchema: "dbo",
						principalTable: "Sexos",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Fichas_SituacionesAdministrativas_SituacionAdministrativaId",
						column: x => x.SituacionAdministrativaId,
						principalSchema: "dbo",
						principalTable: "SituacionesAdministrativas",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "Adjuntos",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					TipoId = table.Column<int>(nullable: false),
					UsuarioId = table.Column<int>(nullable: true),
					FichaId = table.Column<int>(nullable: true),
					OrganizacionId = table.Column<int>(nullable: true),
					Alias = table.Column<string>(maxLength: 255, nullable: false),
					NombreOriginal = table.Column<string>(maxLength: 255, nullable: false),
					Tamano = table.Column<long>(nullable: false),
					FechaAlta = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Adjuntos", x => x.Id);
					table.ForeignKey(
						name: "FK_Adjuntos_Fichas_FichaId",
						column: x => x.FichaId,
						principalSchema: "dbo",
						principalTable: "Fichas",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Adjuntos_TiposAdjunto_TipoId",
						column: x => x.TipoId,
						principalSchema: "dbo",
						principalTable: "TiposAdjunto",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Organizaciones",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					FotoId = table.Column<int>(nullable: true),
					Nombre = table.Column<string>(maxLength: 150, nullable: false),
					Email = table.Column<string>(maxLength: 250, nullable: true),
					Telefono = table.Column<string>(maxLength: 20, nullable: true),
					Web = table.Column<string>(maxLength: 250, nullable: true),
					Activa = table.Column<bool>(nullable: false),
					Observaciones = table.Column<string>(nullable: true),
					FechaAlta = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Organizaciones", x => x.Id);
					table.ForeignKey(
						name: "FK_Organizaciones_Adjuntos_FotoId",
						column: x => x.FotoId,
						principalSchema: "dbo",
						principalTable: "Adjuntos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Usuarios",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					FotoId = table.Column<int>(nullable: true),
					OrganizacionId = table.Column<int>(nullable: false),
					Email = table.Column<string>(maxLength: 250, nullable: false),
					Telefono = table.Column<string>(maxLength: 20, nullable: true),
					Nombre = table.Column<string>(maxLength: 150, nullable: true),
					Apellidos = table.Column<string>(maxLength: 150, nullable: true),
					Hash = table.Column<string>(maxLength: 128, nullable: false),
					IntentosLogin = table.Column<int>(nullable: false),
					Activo = table.Column<bool>(nullable: false),
					Observaciones = table.Column<string>(nullable: true),
					UltimoAcceso = table.Column<DateTime>(nullable: true),
					FinBloqueo = table.Column<DateTime>(nullable: true),
					FechaAlta = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Usuarios", x => x.Id);
					table.ForeignKey(
						name: "FK_Usuarios_Adjuntos_FotoId",
						column: x => x.FotoId,
						principalSchema: "dbo",
						principalTable: "Adjuntos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Usuarios_Organizaciones_OrganizacionId",
						column: x => x.OrganizacionId,
						principalSchema: "dbo",
						principalTable: "Organizaciones",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Logs",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<long>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UserId = table.Column<int>(nullable: true),
					Date = table.Column<DateTime>(nullable: false),
					Logger = table.Column<string>(nullable: true),
					Level = table.Column<string>(nullable: true),
					LevelOrder = table.Column<int>(nullable: false),
					Exception = table.Column<string>(nullable: true),
					CallSite = table.Column<string>(nullable: true),
					Message = table.Column<string>(nullable: true),
					Ip = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Logs", x => x.Id);
					table.ForeignKey(
						name: "FK_Logs_Usuarios_UserId",
						column: x => x.UserId,
						principalSchema: "dbo",
						principalTable: "Usuarios",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Seguimientos",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					UsuarioId = table.Column<int>(nullable: false),
					FichaId = table.Column<int>(nullable: false),
					Fecha = table.Column<DateTime>(nullable: false),
					Observaciones = table.Column<string>(nullable: true),
					Completo = table.Column<bool>(nullable: false),
					FechaAlta = table.Column<DateTime>(nullable: false),
					FechaUltimaModificacion = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Seguimientos", x => x.Id);
					table.ForeignKey(
						name: "FK_Seguimientos_Fichas_FichaId",
						column: x => x.FichaId,
						principalSchema: "dbo",
						principalTable: "Fichas",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Seguimientos_Usuarios_UsuarioId",
						column: x => x.UsuarioId,
						principalSchema: "dbo",
						principalTable: "Usuarios",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
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
				name: "UsuariosPermisos",
				schema: "dbo",
				columns: table => new
				{
					UsuarioId = table.Column<int>(nullable: false),
					PermisoId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UsuariosPermisos", x => new { x.UsuarioId, x.PermisoId });
					table.ForeignKey(
						name: "FK_UsuariosPermisos_Permisos_PermisoId",
						column: x => x.PermisoId,
						principalSchema: "dbo",
						principalTable: "Permisos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_UsuariosPermisos_Usuarios_UsuarioId",
						column: x => x.UsuarioId,
						principalSchema: "dbo",
						principalTable: "Usuarios",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IndicadoresSeguimientos",
				schema: "dbo",
				columns: table => new
				{
					IndicadorId = table.Column<int>(nullable: false),
					SeguimientoId = table.Column<int>(nullable: false),
					Observaciones = table.Column<string>(nullable: true),
					Verificado = table.Column<bool>(nullable: true)
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
				name: "IX_Adjuntos_FichaId",
				schema: "dbo",
				table: "Adjuntos",
				column: "FichaId");

			migrationBuilder.CreateIndex(
				name: "IX_Adjuntos_TipoId",
				schema: "dbo",
				table: "Adjuntos",
				column: "TipoId");

			migrationBuilder.CreateIndex(
				name: "IX_Categorias_DimensionId",
				schema: "dbo",
				table: "Categorias",
				column: "DimensionId");

			migrationBuilder.CreateIndex(
				name: "IX_Dimensiones_IconoId",
				schema: "dbo",
				table: "Dimensiones",
				column: "IconoId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_FotoId",
				schema: "dbo",
				table: "Fichas",
				column: "FotoId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_GeneroId",
				schema: "dbo",
				table: "Fichas",
				column: "GeneroId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_MunicipioId",
				schema: "dbo",
				table: "Fichas",
				column: "MunicipioId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_NacionalidadId",
				schema: "dbo",
				table: "Fichas",
				column: "NacionalidadId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_OrganizacionId",
				schema: "dbo",
				table: "Fichas",
				column: "OrganizacionId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_OrigenId",
				schema: "dbo",
				table: "Fichas",
				column: "OrigenId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_PadronId",
				schema: "dbo",
				table: "Fichas",
				column: "PadronId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_ProvinciaId",
				schema: "dbo",
				table: "Fichas",
				column: "ProvinciaId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_SexoId",
				schema: "dbo",
				table: "Fichas",
				column: "SexoId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_SituacionAdministrativaId",
				schema: "dbo",
				table: "Fichas",
				column: "SituacionAdministrativaId");

			migrationBuilder.CreateIndex(
				name: "IX_Fichas_UsuarioId",
				schema: "dbo",
				table: "Fichas",
				column: "UsuarioId");

			migrationBuilder.CreateIndex(
				name: "IX_Indicadores_CategoriaId",
				schema: "dbo",
				table: "Indicadores",
				column: "CategoriaId");

			migrationBuilder.CreateIndex(
				name: "IX_IndicadoresSeguimientos_IndicadorId",
				schema: "dbo",
				table: "IndicadoresSeguimientos",
				column: "IndicadorId");

			migrationBuilder.CreateIndex(
				name: "IX_Logs_UserId",
				schema: "dbo",
				table: "Logs",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Municipios_ProvinciaId",
				schema: "dbo",
				table: "Municipios",
				column: "ProvinciaId");

			migrationBuilder.CreateIndex(
				name: "IX_Organizaciones_FotoId",
				schema: "dbo",
				table: "Organizaciones",
				column: "FotoId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_PerfilesPermisos_PermisoId",
				schema: "dbo",
				table: "PerfilesPermisos",
				column: "PermisoId");

			migrationBuilder.CreateIndex(
				name: "IX_Seguimientos_FichaId",
				schema: "dbo",
				table: "Seguimientos",
				column: "FichaId");

			migrationBuilder.CreateIndex(
				name: "IX_Seguimientos_UsuarioId",
				schema: "dbo",
				table: "Seguimientos",
				column: "UsuarioId");

			migrationBuilder.CreateIndex(
				name: "IX_Usuarios_FotoId",
				schema: "dbo",
				table: "Usuarios",
				column: "FotoId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Usuarios_OrganizacionId",
				schema: "dbo",
				table: "Usuarios",
				column: "OrganizacionId");

			migrationBuilder.CreateIndex(
				name: "IX_UsuariosPerfiles_PerfilId",
				schema: "dbo",
				table: "UsuariosPerfiles",
				column: "PerfilId");

			migrationBuilder.CreateIndex(
				name: "IX_UsuariosPermisos_PermisoId",
				schema: "dbo",
				table: "UsuariosPermisos",
				column: "PermisoId");

			migrationBuilder.AddForeignKey(
				name: "FK_Dimensiones_Adjuntos_IconoId",
				schema: "dbo",
				table: "Dimensiones",
				column: "IconoId",
				principalSchema: "dbo",
				principalTable: "Adjuntos",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Fichas_Adjuntos_FotoId",
				schema: "dbo",
				table: "Fichas",
				column: "FotoId",
				principalSchema: "dbo",
				principalTable: "Adjuntos",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Fichas_Organizaciones_OrganizacionId",
				schema: "dbo",
				table: "Fichas",
				column: "OrganizacionId",
				principalSchema: "dbo",
				principalTable: "Organizaciones",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Fichas_Usuarios_UsuarioId",
				schema: "dbo",
				table: "Fichas",
				column: "UsuarioId",
				principalSchema: "dbo",
				principalTable: "Usuarios",
				principalColumn: "Id");

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Configuraciones",
			   columns: new[] { "Enlaces", "Id", "MostrarEnlaces" },
			   values: new object[,]
			   {{"Programa Incorpora|https://intranet.incorpora.org/Incorpora/;",1,true}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Dimensiones",
			   columns: new[] { "Id", "Orden", "Descripcion", "Activo" },
			   values: new object[,]
			   {{1,1,"Vivienda",true},{2,2,"Salud",true},{3,3,"Factores Estructurales",true},{4,4,"Habilidades personales y sociales",true},{5,5,"Derechos Civiles y Políticos",true},{6,6,"Situación económica",true},{7,7,"Situación educativa y formativa",true},{8,8,"Situación Ocupacional",true},{9,9,"Situación Personal y Familiar",true}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Empadronamientos",
			   columns: new[] { "Id", "Descripcion" },
			   values: new object[,]
			   {{1,"Sí"},{2,"No"},{3,"En trámite"}});

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "Perfiles",
				columns: new[] { "Id", "Descripcion" },
				values: new object[,]
				{{1,"Usuario"},{2,"Administrador"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Permisos",
			   columns: new[] { "Id", "Descripcion", "Clave" },
			   values: new object[,]
			   {{2,"Usuarios: Lectura","usermng:read"},{3,"Usuarios: Escritura","usermng:write"},{4,"Usuarios: Eliminar","usermng:delete"},{5,"Usuarios: Acceder","usermng:access"},{6,"Aplicación: Superadministrador","user:superadmin"},{7,"Fichas: Lectura","card:read"},{8,"Fichas: Escritura","card:write"},{9,"Fichas: Eliminar","card:delete"},{10,"Fichas: Acceder","card:access"},{11,"Adjuntos: Lectura","attachments:read"},{12,"Adjuntos: Escritura","attachments:write"},{13,"Adjuntos: Eliminar","attachments:delete"},{14,"Adjuntos: Acceder","attachments:access"},{15,"Seguimientos: Lectura","indicators:read"},{16,"Seguimientos: Escritura","indicators:write"},{17,"Seguimientos: Eliminar","indicators:delete"},{18,"Seguimientos: Acceder","indicators:access"},{19,"Estadísticas: Acceso","stats:access"},{20,"Estadísticas: Datos Globales","stats:global"},{21,"Mi Organización: Gestionar","user:partmang"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "PerfilesPermisos",
			   columns: new[] { "PerfilId", "PermisoId" },
			   values: new object[,]
			   {{2,2},{2,3},{2,4},{2,5}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Provincias",
			   columns: new[] { "Id", "Nombre" },
			   values: new object[,]
			   {{1,"Araba/Álava"},{2,"Albacete"},{3,"Alicante/Alacant"},{4,"Almería"},{5,"Ávila"},{6,"Badajoz"},{7,"Balears, Illes"},{8,"Barcelona"},{9,"Burgos"},{10,"Cáceres"},{11,"Cádiz"},{12,"Castellón/Castelló"},{13,"Ciudad Real"},{14,"Córdoba"},{15,"Coruña, A"},{16,"Cuenca"},{17,"Girona"},{18,"Granada"},{19,"Guadalajara"},{20,"Gipuzkoa"},{21,"Huelva"},{22,"Huesca"},{23,"Jaén"},{24,"León"},{25,"Lleida"},{26,"Rioja, La"},{27,"Lugo"},{28,"Madrid"},{29,"Málaga"},{30,"Murcia"},{31,"Navarra"},{32,"Ourense"},{33,"Asturias"},{34,"Palencia"},{35,"Palmas, Las"},{36,"Pontevedra"},{37,"Salamanca"},{38,"Santa Cruz de Tenerife"},{39,"Cantabria"},{40,"Segovia"},{41,"Sevilla"},{42,"Soria"},{43,"Tarragona"},{44,"Teruel"},{45,"Toledo"},{46,"Valencia/València"},{47,"Valladolid"},{48,"Bizkaia"},{49,"Zamora"},{50,"Zaragoza"},{51,"Ceuta"},{52,"Melilla"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Rangos",
			   columns: new[] { "Id", "Descripcion", "Minimo", "Maximo" },
			   values: new object[,]
			   {{1,"Sin Vulnerabilidad",0,9},{2,"Vulnerabilidad leve",10,19},{3,"Vulnerabilidad moderada",20,29},{4,"Vulnerabilidad grave",30,44},{5,"Vulnerabilidad extrema",45, null}});

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "Sexos",
				columns: new[] { "Id", "Descripcion" },
				values: new object[,]
				{{1,"Hombre"},{2,"Mujer"},{3,"Otros"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "SituacionesAdministrativas",
			   columns: new[] { "Id", "Descripcion" },
			   values: new object[,]
			   {{1,"Permiso de trabajo y residencia"},{2,"Permiso de residencia"},{3,"Situación irregular"},{4,"Protección internacional"},{5,"Solicitante de Protección internacional"},{6,"NIE ciudadanos comunitarios"},{7,"DNI"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "TiposAdjunto",
			   columns: new[] { "Id", "Descripcion", "Carpeta" },
			   values: new object[,]
			   {{1,"Imagenes","images"},{2,"Adjuntos","attachments"},{3,"Documentacion","docs"},{4,"Personales Ficha","personal"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Paises",
			   columns: new[] { "Descripcion", "Id" },
			   values: new object[,]
			   {{"Afganistán",1},{"Albania",2},{"Alemania",3},{"Andorra",4},{"Angola",5},{"Antigua y Barbuda",6},{"Arabia Saudita",7},{"Argelia",8},{"Argentina",9},{"Armenia",10},{"Australia",11},{"Austria",12},{"Azerbaiyán",13},{"Bahamas",14},{"Bangladés",15},{"Barbados",16},{"Baréin",17},{"Bélgica",18},{"Belice",19},{"Benín",20},{"Bielorrusia",21},{"Birmania",22},{"Bolivia",23},{"Bosnia y Herzegovina",24},{"Botsuana",25},{"Brasil",26},{"Brunéi",27},{"Bulgaria",28},{"Burkina Faso",29},{"Burundi",30},{"Bután",31},{"Cabo Verde",32},{"Camboya",33},{"Camerún",34},{"Canadá",35},{"Catar",36},{"Chad",37},{"Chile",38},{"China",39},{"Chipre",40},{"Ciudad del Vaticano",41},{"Colombia",42},{"Comoras",43},{"Corea del Norte",44},{"Corea del Sur",45},{"Costa de Marfil",46},{"Costa Rica",47},{"Croacia",48},{"Cuba",49},{"Dinamarca",50},{"Dominica",51},{"Ecuador",52},{"Egipto",53},{"El Salvador",54},{"Emiratos Árabes Unidos",55},{"Eritrea",56},{"Eslovaquia",57},{"Eslovenia",58},{"España",59},{"Estados Unidos",60},{"Estonia",61},{"Etiopía",62},{"Filipinas",63},{"Finlandia",64},{"Fiyi",65},{"Francia",66},{"Gabón",67},{"Gambia",68},{"Georgia",69},{"Ghana",70},{"Granada",71},{"Grecia",72},{"Guatemala",73},{"Guyana",74},{"Guinea",75},{"Guinea-Bisáu",76},{"Guinea Ecuatorial",77},{"Haití",78},{"Honduras",79},{"Hungría",80},{"India",81},{"Indonesia",82},{"Irak",83},{"Irán",84},{"Irlanda",85},{"Islandia",86},{"Islas Marshall",87},{"Islas Salomón",88},{"Israel",89},{"Italia",90},{"Jamaica",91},{"Japón",92},{"Jordania",93},{"Kazajistán",94},{"Kenia",95},{"Kirguistán",96},{"Kiribati",97},{"Kuwait",98},{"Laos",99},{"Lesoto",100},{"Letonia",101},{"Líbano",102},{"Liberia",103},{"Libia",104},{"Liechtenstein",105},{"Lituania",106},{"Luxemburgo",107},{"Macedonia del Norte",108},{"Madagascar",109},{"Malasia",110},{"Malaui",111},{"Maldivas",112},{"Malí",113},{"Malta",114},{"Marruecos",115},{"Mauricio",116},{"Mauritania",117},{"México",118},{"Micronesia",119},{"Moldavia",120},{"Mónaco",121},{"Mongolia",122},{"Montenegro",123},{"Mozambique",124},{"Namibia",125},{"Nauru",126},{"Nepal",127},{"Nicaragua",128},{"Níger",129},{"Nigeria",130},{"Noruega",131},{"Nueva Zelanda",132},{"Omán",133},{"Países Bajos",134},{"Pakistán",135},{"Palaos",136},{"Panamá",137},{"Papúa Nueva Guinea",138},{"Paraguay",139},{"Perú",140},{"Polonia",141},{"Portugal",142},{"Reino Unido de Gran Bretaña e Irlanda del Norte",143},{"República Centroafricana",144},{"República Checa",145},{"República del Congo",146},{"República Democrática del Congo",147},{"República Dominicana",148},{"República Sudafricana",149},{"Ruanda",150},{"Rumanía",151},{"Rusia",152},{"Samoa",153},{"San Cristóbal y Nieves",154},{"San Marino",155},{"San Vicente y las Granadinas",156},{"Santa Lucía",157},{"Santo Tomé y Príncipe",158},{"Senegal",159},{"Serbia",160},{"Seychelles",161},{"Sierra Leona",162},{"Singapur",163},{"Siria",164},{"Somalia",165},{"Sri Lanka",166},{"Suazilandia",167},{"Sudán",168},{"Sudán del Sur",169},{"Suecia",170},{"Suiza",171},{"Surinam",172},{"Tailandia",173},{"Tanzania",174},{"Tayikistán",175},{"Timor Oriental",176},{"Togo",177},{"Tonga",178},{"Trinidad y Tobago",179},{"Túnez",180},{"Turkmenistán",181},{"Turquía",182},{"Tuvalu",183},{"Ucrania",184},{"Uganda",185},{"Uruguay",186},{"Uzbekistán",187},{"Vanuatu",188},{"Venezuela",189},{"Vietnam",190},{"Yemen",191},{"Yibuti",192},{"Zambia",193},{"Zimbabue",194}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Categorias",
			   columns: new[] { "Id", "Orden", "DimensionId", "Descripcion", "Activo", "Obligatorio", "RespuestaMultiple" },
			   values: new object[,]
			   {{1,1,1,"Vivienda Segura",true,false,false},{2,2,1,"Vivienda Insegura",true,false,false},{3,3,1,"Vivienda Inadecuada",true,false,true},{4,4,1,"Sin Vivienda ",true,false,false},{5,5,1,"Sin Techo",true,false,false},{6,1,2,"Adicciones",true,false,false},{7,2,2,"Salud física",true,false,false},{8,3,2,"Salud mental",true,false,false},{9,4,2,"Adherencia a tratamiento médico",true,false,false},{10,5,2,"Discapacidad, Dependencia e Incapacidad",true,false,false},{11,6,2,"Acceso a atención sanitaria",true,false,false},{12,1,3,"Factores Ambientales",true,false,true},{13,2,3,"Factores Institucionales",true,false,false},{14,3,4,"Habilidades personales y sociales",true,false,true},{15,4,4,"Competencias cognitivas básicas",true,false,false},{16,5,4,"Competencias Instrumentales",true,false,true},{17,1,5,"Relación con el sistema penal",true,false,false},{18,2,5,"Extranjería",true,false,false},{19,3,5,"Discriminación por identidad de género o sexual ",true,false,false},{20,4,5,"Discriminación por raza o etnia",true,false,false},{21,5,5,"Discriminación por adscripción religiosa",true,false,false},{22,6,5,"Discriminación por situación de salud (VIH, discapacidad, etc.)",true,false,false},{23,7,5,"Empadronamiento",true,false,false},{24,1,6,"Carencia o insuficiencia de ingresos",true,false,false},{25,2,6,"Privación material del hogar",true,false,false},{26,3,6,"Nivel de endeudamiento ",true,false,false},{27,4,6,"Tipo de deuda",true,false,false},{28,5,6,"Fuentes de ingresos",true,false,false},{29,1,7,"Escolarización de menores",true,false,false},{30,2,7,"Nivel de Estudios",true,false,false},{31,3,7,"Competencias",true,false,true},{32,4,7,"Homologación de estudios para personas extranjeras",true,false,false},{33,5,8,"Empleabilidad",true,false,false},{34,6,8,"Desempleados/as parados/as",true,false,false},{35,7,8,"Ocupados/as",true,false,false},{36,1,9,"Edad",true,false,false},{37,2,9,"Tutela",true,false,false},{38,3,9,"Violencia de género",true,false,false},{39,4,9,"Maltrato infantil",true,false,false},{40,5,9,"Maltrato a mayores",true,false,false},{41,6,9,"Responsabilidades familiares",true,false,false},{42,8,9,"Hacinamiento ",true,false,false},{43,9,9,"Convivencia",true,false,true},{44,10,9,"Apoyo social y/o familiar",true,false,false},{45,7,9,"Responsabilidades familiares (cont.)",true,false,false},{46,11,9,"Apoyo social y/o familiar (cont.)",true,false,false}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "Municipios",
			   columns: new[] { "Id", "ProvinciaId", "Nombre" },
			   values: new object[,]
			   {
					{ 1, 1, "Alegría-Dulantzi" },
					{ 2, 2, "Abengibre" },
					{ 3, 3, "Adsubia" },
					{ 4, 4, "Abla" },
					{ 5, 5, "Adanero" },
					{ 6, 6, "Acedera" },
					{ 7, 7, "Alaró" },
					{ 8, 8, "Abrera" },
					{ 9, 9, "Abajas" },
					{ 10, 10, "Abadía" },
					{ 11, 11, "Alcalá de los Gazules" },
					{ 12, 12, "Atzeneta del Maestrat" },
					{ 13, 13, "Abenójar" },
					{ 14, 14, "Adamuz" },
					{ 15, 15, "Abegondo" },
					{ 16, 16, "Abia de la Obispalía" },
					{ 17, 17, "Agullana" },
					{ 18, 18, "Agrón" },
					{ 19, 19, "Abánades" },
					{ 20, 20, "Abaltzisketa" },
					{ 21, 21, "Alájar" },
					{ 22, 22, "Abiego" },
					{ 23, 23, "Albanchez de Mágina" },
					{ 24, 24, "Acebedo" },
					{ 25, 25, "Abella de la Conca" },
					{ 26, 26, "Ábalos" },
					{ 27, 27, "Abadín" },
					{ 28, 28, "Acebeda, La" },
					{ 29, 29, "Alameda" },
					{ 30, 30, "Abanilla" },
					{ 31, 31, "Abáigar" },
					{ 32, 32, "Allariz" },
					{ 33, 33, "Allande" },
					{ 34, 34, "Abarca de Campos" },
					{ 35, 35, "Agaete" },
					{ 36, 36, "Arbo" },
					{ 37, 37, "Abusejo" },
					{ 38, 38, "Adeje" },
					{ 39, 39, "Alfoz de Lloredo" },
					{ 40, 40, "Abades" },
					{ 41, 41, "Aguadulce" },
					{ 42, 42, "Abejar" },
					{ 43, 43, "Aiguamúrcia" },
					{ 44, 44, "Ababuj" },
					{ 45, 45, "Ajofrín" },
					{ 46, 46, "Ademuz" },
					{ 47, 47, "Adalia" },
					{ 48, 48, "Abadiño" },
					{ 49, 50, "Abanto" },
					{ 50, 51, "Ceuta" },
					{ 51, 52, "Melilla" },
					{ 52, 1, "Amurrio" },
					{ 53, 2, "Alatoz" },
					{ 54, 3, "Agost" },
					{ 55, 4, "Abrucena" },
					{ 56, 5, "Adrada, La" },
					{ 57, 6, "Aceuchal" },
					{ 58, 7, "Alaior" },
					{ 59, 8, "Aguilar de Segarra" },
					{ 60, 10, "Abertura" },
					{ 61, 11, "Alcalá del Valle" },
					{ 62, 12, "Aín" },
					{ 63, 13, "Agudo" },
					{ 64, 14, "Aguilar de la Frontera" },
					{ 65, 15, "Ames" },
					{ 66, 16, "Acebrón, El" },
					{ 67, 17, "Aiguaviva" },
					{ 68, 18, "Alamedilla" },
					{ 69, 19, "Ablanque" },
					{ 70, 20, "Aduna" },
					{ 71, 21, "Aljaraque" },
					{ 72, 22, "Abizanda" },
					{ 73, 23, "Alcalá la Real" },
					{ 74, 24, "Algadefe" },
					{ 75, 25, "Àger" },
					{ 76, 26, "Agoncillo" },
					{ 77, 27, "Alfoz" },
					{ 78, 28, "Ajalvir" },
					{ 79, 29, "Alcaucín" },
					{ 80, 30, "Abarán" },
					{ 81, 31, "Abárzuza/Abartzuza" },
					{ 82, 32, "Amoeiro" },
					{ 83, 33, "Aller" },
					{ 84, 35, "Agüimes" },
					{ 85, 36, "Barro" },
					{ 86, 37, "Agallas" },
					{ 87, 38, "Agulo" },
					{ 88, 39, "Ampuero" },
					{ 89, 40, "Adrada de Pirón" },
					{ 90, 41, "Alanís" },
					{ 91, 43, "Albinyana" },
					{ 92, 44, "Abejuela" },
					{ 93, 45, "Alameda de la Sagra" },
					{ 94, 46, "Ador" },
					{ 95, 47, "Aguasal" },
					{ 96, 48, "Abanto y Ciérvana-Abanto Zierbena" },
					{ 97, 49, "Abezames" },
					{ 98, 50, "Acered" },
					{ 99, 1, "Aramaio" },
					{ 100, 2, "Albacete" },
					{ 101, 3, "Agres" },
					{ 102, 4, "Adra" },
					{ 103, 6, "Ahillones" },
					{ 104, 7, "Alcúdia" },
					{ 105, 8, "Alella" },
					{ 106, 9, "Adrada de Haza" },
					{ 107, 10, "Acebo" },
					{ 108, 11, "Algar" },
					{ 109, 12, "Albocàsser" },
					{ 110, 13, "Alamillo" },
					{ 111, 14, "Alcaracejos" },
					{ 112, 15, "Aranga" },
					{ 113, 16, "Alarcón" },
					{ 114, 17, "Albanyà" },
					{ 115, 18, "Albolote" },
					{ 116, 19, "Adobes" },
					{ 117, 20, "Aizarnazabal" },
					{ 118, 21, "Almendro, El" },
					{ 119, 22, "Adahuesca" },
					{ 120, 23, "Alcaudete" },
					{ 121, 24, "Alija del Infantado" },
					{ 122, 25, "Agramunt" },
					{ 123, 26, "Aguilar del Río Alhama" },
					{ 124, 27, "Antas de Ulla" },
					{ 125, 28, "Alameda del Valle" },
					{ 126, 29, "Alfarnate" },
					{ 127, 30, "Águilas" },
					{ 128, 31, "Abaurregaina/Abaurrea Alta" },
					{ 129, 32, "Arnoia, A" },
					{ 130, 33, "Amieva" },
					{ 131, 34, "Abia de las Torres" },
					{ 132, 35, "Antigua" },
					{ 133, 36, "Baiona" },
					{ 134, 37, "Ahigal de los Aceiteros" },
					{ 135, 38, "Alajeró" },
					{ 136, 39, "Anievas" },
					{ 137, 40, "Adrados" },
					{ 138, 41, "Albaida del Aljarafe" },
					{ 139, 42, "Adradas" },
					{ 140, 43, "Albiol, L\'" },
					{ 141, 44, "Aguatón" },
					{ 142, 45, "Albarreal de Tajo" },
					{ 143, 46, "Atzeneta d\'Albaida" },
					{ 144, 47, "Aguilar de Campos" },
					{ 145, 48, "Amorebieta-Etxano" },
					{ 146, 49, "Alcañices" },
					{ 147, 50, "Agón" },
					{ 148, 1, "Artziniega" },
					{ 149, 2, "Albatana" },
					{ 150, 3, "Aigües" },
					{ 151, 4, "Albánchez" },
					{ 152, 6, "Alange" },
					{ 153, 7, "Algaida" },
					{ 154, 8, "Alpens" },
					{ 155, 10, "Acehúche" },
					{ 156, 11, "Algeciras" },
					{ 157, 12, "Alcalà de Xivert" },
					{ 158, 13, "Albaladejo" },
					{ 159, 14, "Almedinilla" },
					{ 160, 15, "Ares" },
					{ 161, 16, "Albaladejo del Cuende" },
					{ 162, 17, "Albons" },
					{ 163, 18, "Albondón" },
					{ 164, 19, "Alaminos" },
					{ 165, 20, "Albiztur" },
					{ 166, 21, "Almonaster la Real" },
					{ 167, 22, "Agüero" },
					{ 168, 23, "Aldeaquemada" },
					{ 169, 24, "Almanza" },
					{ 170, 25, "Alamús, Els" },
					{ 171, 26, "Ajamil de Cameros" },
					{ 172, 27, "Baleira" },
					{ 173, 28, "Álamo, El" },
					{ 174, 29, "Alfarnatejo" },
					{ 175, 30, "Albudeite" },
					{ 176, 31, "Abaurrepea/Abaurrea Baja" },
					{ 177, 32, "Avión" },
					{ 178, 33, "Avilés" },
					{ 179, 34, "Aguilar de Campoo" },
					{ 180, 35, "Arrecife" },
					{ 181, 36, "Bueu" },
					{ 182, 37, "Ahigal de Villarino" },
					{ 183, 38, "Arafo" },
					{ 184, 39, "Arenas de Iguña" },
					{ 185, 40, "Aguilafuente" },
					{ 186, 41, "Alcalá de Guadaíra" },
					{ 187, 42, "Ágreda" },
					{ 188, 43, "Alcanar" },
					{ 189, 44, "Aguaviva" },
					{ 190, 45, "Alcabón" },
					{ 191, 46, "Agullent" },
					{ 192, 47, "Alaejos" },
					{ 193, 48, "Amoroto" },
					{ 194, 49, "Alcubilla de Nogales" },
					{ 195, 50, "Aguarón" },
					{ 196, 2, "Alborea" },
					{ 197, 3, "Albatera" },
					{ 198, 4, "Alboloduy" },
					{ 199, 5, "Albornos" },
					{ 200, 6, "Albuera, La" },
					{ 201, 7, "Andratx" },
					{ 202, 8, "Ametlla del Vallès, L\'" },
					{ 203, 10, "Aceituna" },
					{ 204, 11, "Algodonales" },
					{ 205, 12, "Alcora, l\'" },
					{ 206, 13, "Alcázar de San Juan" },
					{ 207, 14, "Almodóvar del Río" },
					{ 208, 15, "Arteixo" },
					{ 209, 16, "Albalate de las Nogueras" },
					{ 210, 17, "Far d\'Empordà, El" },
					{ 211, 18, "Albuñán" },
					{ 212, 19, "Alarilla" },
					{ 213, 20, "Alegia" },
					{ 214, 21, "Almonte" },
					{ 215, 23, "Andújar" },
					{ 216, 24, "Antigua, La" },
					{ 217, 25, "Alàs i Cerc" },
					{ 218, 26, "Albelda de Iregua" },
					{ 219, 27, "Barreiros" },
					{ 220, 28, "Alcalá de Henares" },
					{ 221, 29, "Algarrobo" },
					{ 222, 30, "Alcantarilla" },
					{ 223, 31, "Aberin" },
					{ 224, 32, "Baltar" },
					{ 225, 33, "Belmonte de Miranda" },
					{ 226, 34, "Alar del Rey" },
					{ 227, 35, "Artenara" },
					{ 228, 36, "Caldas de Reis" },
					{ 229, 37, "Alameda de Gardón, La" },
					{ 230, 38, "Arico" },
					{ 231, 39, "Argoños" },
					{ 232, 40, "Alconada de Maderuelo" },
					{ 233, 41, "Alcalá del Río" },
					{ 234, 43, "Alcover" },
					{ 235, 44, "Aguilar del Alfambra" },
					{ 236, 45, "Alcañizo" },
					{ 237, 46, "Alaquàs" },
					{ 238, 47, "Alcazarén" },
					{ 239, 48, "Arakaldo" },
					{ 240, 49, "Alfaraz de Sayago" },
					{ 241, 50, "Aguilón" },
					{ 242, 1, "Armiñón" },
					{ 243, 2, "Alcadozo" },
					{ 244, 3, "Alcalalí" },
					{ 245, 4, "Albox" },
					{ 246, 6, "Alburquerque" },
					{ 247, 7, "Artà" },
					{ 248, 8, "Arenys de Mar" },
					{ 249, 9, "Aguas Cándidas" },
					{ 250, 10, "Ahigal" },
					{ 251, 11, "Arcos de la Frontera" },
					{ 252, 12, "Alcudia de Veo" },
					{ 253, 13, "Alcoba" },
					{ 254, 14, "Añora" },
					{ 255, 15, "Arzúa" },
					{ 256, 16, "Albendea" },
					{ 257, 17, "Alp" },
					{ 258, 18, "Albuñol" },
					{ 259, 19, "Albalate de Zorita" },
					{ 260, 20, "Alkiza" },
					{ 261, 21, "Alosno" },
					{ 262, 22, "Aisa" },
					{ 263, 23, "Arjona" },
					{ 264, 24, "Ardón" },
					{ 265, 25, "Albagés, L\'" },
					{ 266, 26, "Alberite" },
					{ 267, 27, "Becerreá" },
					{ 268, 28, "Alcobendas" },
					{ 269, 29, "Algatocín" },
					{ 270, 30, "Aledo" },
					{ 271, 31, "Ablitas" },
					{ 272, 32, "Bande" },
					{ 273, 33, "Bimenes" },
					{ 274, 34, "Alba de Cerrato" },
					{ 275, 35, "Arucas" },
					{ 276, 36, "Cambados" },
					{ 277, 37, "Alamedilla, La" },
					{ 278, 38, "Arona" },
					{ 279, 39, "Arnuero" },
					{ 280, 40, "Aldealcorvo" },
					{ 281, 41, "Alcolea del Río" },
					{ 282, 42, "Alconaba" },
					{ 283, 43, "Aldover" },
					{ 284, 44, "Alacón" },
					{ 285, 45, "Alcaudete de la Jara" },
					{ 286, 46, "Albaida" },
					{ 287, 47, "Aldea de San Miguel" },
					{ 288, 48, "Arantzazu" },
					{ 289, 49, "Algodre" },
					{ 290, 50, "Ainzón" },
					{ 291, 2, "Alcalá del Júcar" },
					{ 292, 3, "Alcocer de Planes" },
					{ 293, 4, "Alcolea" },
					{ 294, 5, "Aldeanueva de Santa Cruz" },
					{ 295, 6, "Alconchel" },
					{ 296, 7, "Banyalbufar" },
					{ 297, 8, "Arenys de Munt" },
					{ 298, 9, "Aguilar de Bureba" },
					{ 299, 10, "Albalá" },
					{ 300, 11, "Barbate" },
					{ 301, 12, "Alfondeguilla" },
					{ 302, 13, "Alcolea de Calatrava" },
					{ 303, 14, "Baena" },
					{ 304, 15, "Baña, A" },
					{ 305, 16, "Alberca de Záncara, La" },
					{ 306, 17, "Amer" },
					{ 307, 18, "Albuñuelas" },
					{ 308, 19, "Albares" },
					{ 309, 20, "Altzo" },
					{ 310, 21, "Aracena" },
					{ 311, 22, "Albalate de Cinca" },
					{ 312, 23, "Arjonilla" },
					{ 313, 24, "Arganza" },
					{ 314, 25, "Albatàrrec" },
					{ 315, 26, "Alcanadre" },
					{ 316, 27, "Begonte" },
					{ 317, 28, "Alcorcón" },
					{ 318, 29, "Alhaurín de la Torre" },
					{ 319, 30, "Alguazas" },
					{ 320, 31, "Adiós" },
					{ 321, 32, "Baños de Molgas" },
					{ 322, 33, "Boal" },
					{ 323, 35, "Betancuria" },
					{ 324, 36, "Campo Lameiro" },
					{ 325, 37, "Alaraz" },
					{ 326, 38, "Barlovento" },
					{ 327, 39, "Arredondo" },
					{ 328, 40, "Aldealengua de Pedraza" },
					{ 329, 41, "Algaba, La" },
					{ 330, 42, "Alcubilla de Avellaneda" },
					{ 331, 43, "Aleixar, L\'" },
					{ 332, 44, "Alba" },
					{ 333, 45, "Alcolea de Tajo" },
					{ 334, 46, "Albal" },
					{ 335, 47, "Aldeamayor de San Martín" },
					{ 336, 48, "Munitibar-Arbatzegi Gerrikaitz" },
					{ 337, 49, "Almaraz de Duero" },
					{ 338, 50, "Aladrén" },
					{ 339, 1, "Arrazua-Ubarrundia" },
					{ 340, 2, "Alcaraz" },
					{ 341, 3, "Alcoleja" },
					{ 342, 4, "Alcóntar" },
					{ 343, 5, "Aldeaseca" },
					{ 344, 6, "Alconera" },
					{ 345, 7, "Binissalem" },
					{ 346, 8, "Argençola" },
					{ 347, 10, "Alcántara" },
					{ 348, 11, "Barrios, Los" },
					{ 349, 12, "Algimia de Almonacid" },
					{ 350, 13, "Alcubillas" },
					{ 351, 14, "Belalcázar" },
					{ 352, 15, "Bergondo" },
					{ 353, 16, "Alcalá de la Vega" },
					{ 354, 17, "Anglès" },
					{ 355, 19, "Albendiego" },
					{ 356, 20, "Amezketa" },
					{ 357, 21, "Aroche" },
					{ 358, 22, "Albalatillo" },
					{ 359, 23, "Arquillos" },
					{ 360, 24, "Astorga" },
					{ 361, 25, "Albesa" },
					{ 362, 26, "Aldeanueva de Ebro" },
					{ 363, 27, "Bóveda" },
					{ 364, 28, "Aldea del Fresno" },
					{ 365, 29, "Alhaurín el Grande" },
					{ 366, 30, "Alhama de Murcia" },
					{ 367, 31, "Aguilar de Codés" },
					{ 368, 32, "Barbadás" },
					{ 369, 33, "Cabrales" },
					{ 370, 35, "Firgas" },
					{ 371, 36, "Cangas" },
					{ 372, 37, "Alba de Tormes" },
					{ 373, 38, "Breña Alta" },
					{ 374, 39, "Astillero, El" },
					{ 375, 40, "Aldealengua de Santa María" },
					{ 376, 41, "Algámitas" },
					{ 377, 42, "Alcubilla de las Peñas" },
					{ 378, 43, "Alfara de Carles" },
					{ 379, 44, "Albalate del Arzobispo" },
					{ 380, 45, "Aldea en Cabo" },
					{ 381, 46, "Albalat de la Ribera" },
					{ 382, 47, "Almenara de Adaja" },
					{ 383, 48, "Artzentales" },
					{ 384, 49, "Almeida de Sayago" },
					{ 385, 50, "Alagón" },
					{ 386, 1, "Asparrena" },
					{ 387, 2, "Almansa" },
					{ 388, 3, "Alcoy/Alcoi" },
					{ 389, 4, "Alcudia de Monteagud" },
					{ 390, 6, "Aljucén" },
					{ 391, 7, "Búger" },
					{ 392, 8, "Argentona" },
					{ 393, 9, "Albillos" },
					{ 394, 10, "Alcollarín" },
					{ 395, 11, "Benaocaz" },
					{ 396, 12, "Almazora/Almassora" },
					{ 397, 13, "Aldea del Rey" },
					{ 398, 14, "Belmez" },
					{ 399, 15, "Betanzos" },
					{ 400, 16, "Alcantud" },
					{ 401, 17, "Arbúcies" },
					{ 402, 19, "Alcocer" },
					{ 403, 20, "Andoain" },
					{ 404, 21, "Arroyomolinos de León" },
					{ 405, 22, "Albelda" },
					{ 406, 23, "Baeza" },
					{ 407, 24, "Balboa" },
					{ 408, 25, "Albi, L\'" },
					{ 409, 26, "Alesanco" },
					{ 410, 27, "Carballedo" },
					{ 411, 28, "Algete" },
					{ 412, 29, "Almáchar" },
					{ 413, 30, "Archena" },
					{ 414, 31, "Aibar/Oibar" },
					{ 415, 32, "Barco de Valdeorras, O" },
					{ 416, 33, "Cabranes" },
					{ 417, 34, "Amayuelas de Arriba" },
					{ 418, 35, "Gáldar" },
					{ 419, 36, "Cañiza, A" },
					{ 420, 37, "Alba de Yeltes" },
					{ 421, 38, "Breña Baja" },
					{ 422, 39, "Bárcena de Cicero" },
					{ 423, 40, "Aldeanueva de la Serrezuela" },
					{ 424, 41, "Almadén de la Plata" },
					{ 425, 42, "Aldealafuente" },
					{ 426, 43, "Alforja" },
					{ 427, 44, "Albarracín" },
					{ 428, 45, "Aldeanueva de Barbarroya" },
					{ 429, 46, "Albalat dels Sorells" },
					{ 430, 47, "Amusquillo" },
					{ 431, 48, "Arrankudiaga" },
					{ 432, 49, "Andavías" },
					{ 433, 50, "Alarba" },
					{ 434, 1, "Ayala/Aiara" },
					{ 435, 2, "Alpera" },
					{ 436, 3, "Alfafara" },
					{ 437, 4, "Alhabia" },
					{ 438, 5, "Aldehuela, La" },
					{ 439, 6, "Almendral" },
					{ 440, 7, "Bunyola" },
					{ 441, 8, "Artés" },
					{ 442, 9, "Alcocero de Mola" },
					{ 443, 10, "Alcuéscar" },
					{ 444, 11, "Bornos" },
					{ 445, 12, "Almedíjar" },
					{ 446, 13, "Alhambra" },
					{ 447, 14, "Benamejí" },
					{ 448, 15, "Boimorto" },
					{ 449, 16, "Alcázar del Rey" },
					{ 450, 17, "Argelaguer" },
					{ 451, 18, "Aldeire" },
					{ 452, 19, "Alcolea de las Peñas" },
					{ 453, 20, "Anoeta" },
					{ 454, 21, "Ayamonte" },
					{ 455, 23, "Bailén" },
					{ 456, 24, "Bañeza, La" },
					{ 457, 25, "Alcanó" },
					{ 458, 26, "Alesón" },
					{ 459, 27, "Castro de Rei" },
					{ 460, 28, "Alpedrete" },
					{ 461, 29, "Almargen" },
					{ 462, 30, "Beniel" },
					{ 463, 31, "Altsasu/Alsasua" },
					{ 464, 32, "Beade" },
					{ 465, 33, "Candamo" },
					{ 466, 34, "Ampudia" },
					{ 467, 35, "Haría" },
					{ 468, 36, "Catoira" },
					{ 469, 37, "Alberca, La" },
					{ 470, 38, "Buenavista del Norte" },
					{ 471, 39, "Bárcena de Pie de Concha" },
					{ 472, 40, "Aldeanueva del Codonal" },
					{ 473, 41, "Almensilla" },
					{ 474, 42, "Aldealices" },
					{ 475, 43, "Alió" },
					{ 476, 44, "Albentosa" },
					{ 477, 45, "Aldeanueva de San Bartolomé" },
					{ 478, 46, "Albalat dels Tarongers" },
					{ 479, 47, "Arroyo de la Encomienda" },
					{ 480, 48, "Arrieta" },
					{ 481, 49, "Arcenillas" },
					{ 482, 50, "Alberite de San Juan" },
					{ 483, 1, "Baños de Ebro/Mañueta" },
					{ 484, 2, "Ayna" },
					{ 485, 3, "Alfàs del Pi, l\'" },
					{ 486, 4, "Alhama de Almería" },
					{ 487, 6, "Almendralejo" },
					{ 488, 7, "Calvià" },
					{ 489, 8, "Avià" },
					{ 490, 9, "Alfoz de Bricia" },
					{ 491, 10, "Aldeacentenera" },
					{ 492, 11, "Bosque, El" },
					{ 493, 12, "Almenara" },
					{ 494, 13, "Almadén" },
					{ 495, 14, "Blázquez, Los" },
					{ 496, 15, "Boiro" },
					{ 497, 16, "Alcohujate" },
					{ 498, 17, "Armentera, L\'" },
					{ 499, 18, "Alfacar" },
					{ 500, 19, "Alcolea del Pinar" },
					{ 501, 20, "Antzuola" },
					{ 502, 21, "Beas" },
					{ 503, 22, "Albero Alto" },
					{ 504, 23, "Baños de la Encina" },
					{ 505, 24, "Barjas" },
					{ 506, 25, "Alcarràs" },
					{ 507, 26, "Alfaro" },
					{ 508, 27, "Castroverde" },
					{ 509, 28, "Ambite" },
					{ 510, 29, "Almogía" },
					{ 511, 30, "Blanca" },
					{ 512, 31, "Allín/Allin" },
					{ 513, 32, "Beariz" },
					{ 514, 33, "Cangas del Narcea" },
					{ 515, 34, "Amusco" },
					{ 516, 35, "Ingenio" },
					{ 517, 36, "Cerdedo" },
					{ 518, 37, "Alberguería de Argañán, La" },
					{ 519, 38, "Candelaria" },
					{ 520, 39, "Bareyo" },
					{ 521, 41, "Arahal" },
					{ 522, 42, "Aldealpozo" },
					{ 523, 43, "Almoster" },
					{ 524, 44, "Alcaine" },
					{ 525, 45, "Almendral de la Cañada" },
					{ 526, 46, "Alberic" },
					{ 527, 47, "Ataquines" },
					{ 528, 48, "Arrigorriaga" },
					{ 529, 49, "Arcos de la Polvorosa" },
					{ 530, 50, "Albeta" },
					{ 531, 2, "Balazote" },
					{ 532, 3, "Algorfa" },
					{ 533, 4, "Alicún" },
					{ 534, 5, "Amavida" },
					{ 535, 6, "Arroyo de San Serván" },
					{ 536, 7, "Campanet" },
					{ 537, 8, "Avinyó" },
					{ 538, 9, "Alfoz de Santa Gadea" },
					{ 539, 10, "Aldea del Cano" },
					{ 540, 11, "Cádiz" },
					{ 541, 12, "Altura" },
					{ 542, 13, "Almadenejos" },
					{ 543, 14, "Bujalance" },
					{ 544, 15, "Boqueixón" },
					{ 545, 16, "Alconchel de la Estrella" },
					{ 546, 17, "Avinyonet de Puigventós" },
					{ 547, 18, "Algarinejo" },
					{ 548, 20, "Arama" },
					{ 549, 21, "Berrocal" },
					{ 550, 22, "Albero Bajo" },
					{ 551, 23, "Beas de Segura" },
					{ 552, 24, "Barrios de Luna, Los" },
					{ 553, 25, "Alcoletge" },
					{ 554, 26, "Almarza de Cameros" },
					{ 555, 27, "Cervantes" },
					{ 556, 28, "Anchuelo" },
					{ 557, 29, "Álora" },
					{ 558, 30, "Bullas" },
					{ 559, 31, "Allo" },
					{ 560, 32, "Blancos, Os" },
					{ 561, 33, "Cangas de Onís" },
					{ 562, 34, "Antigüedad" },
					{ 563, 35, "Mogán" },
					{ 564, 36, "Cotobade" },
					{ 565, 37, "Alconada" },
					{ 566, 38, "Fasnia" },
					{ 567, 39, "Cabezón de la Sal" },
					{ 568, 40, "Aldea Real" },
					{ 569, 41, "Aznalcázar" },
					{ 570, 42, "Aldealseñor" },
					{ 571, 43, "Altafulla" },
					{ 572, 44, "Alcalá de la Selva" },
					{ 573, 45, "Almonacid de Toledo" },
					{ 574, 46, "Alborache" },
					{ 575, 47, "Bahabón" },
					{ 576, 48, "Bakio" },
					{ 577, 49, "Argañín" },
					{ 578, 50, "Alborge" },
					{ 579, 1, "Barrundia" },
					{ 580, 2, "Balsa de Ves" },
					{ 581, 3, "Algueña" },
					{ 582, 4, "Almería" },
					{ 583, 5, "Arenal, El" },
					{ 584, 6, "Atalaya" },
					{ 585, 7, "Campos" },
					{ 586, 8, "Avinyonet del Penedès" },
					{ 587, 9, "Altable" },
					{ 588, 10, "Aldea del Obispo, La" },
					{ 589, 11, "Castellar de la Frontera" },
					{ 590, 12, "Arañuel" },
					{ 591, 13, "Almagro" },
					{ 592, 14, "Cabra" },
					{ 593, 15, "Brión" },
					{ 594, 16, "Algarra" },
					{ 595, 17, "Begur" },
					{ 596, 18, "Alhama de Granada" },
					{ 597, 19, "Alcoroches" },
					{ 598, 20, "Aretxabaleta" },
					{ 599, 21, "Bollullos Par del Condado" },
					{ 600, 22, "Alberuela de Tubo" },
					{ 601, 25, "Alfarràs" },
					{ 602, 26, "Anguciana" },
					{ 603, 27, "Cervo" },
					{ 604, 28, "Aranjuez" },
					{ 605, 29, "Alozaina" },
					{ 606, 30, "Calasparra" },
					{ 607, 31, "Améscoa Baja" },
					{ 608, 32, "Boborás" },
					{ 609, 33, "Caravia" },
					{ 610, 35, "Moya" },
					{ 611, 36, "Covelo" },
					{ 612, 37, "Aldeacipreste" },
					{ 613, 38, "Frontera" },
					{ 614, 39, "Cabezón de Liébana" },
					{ 615, 40, "Aldeasoña" },
					{ 616, 41, "Aznalcóllar" },
					{ 617, 42, "Aldehuela de Periáñez" },
					{ 618, 43, "Ametlla de Mar, L\'" },
					{ 619, 44, "Alcañiz" },
					{ 620, 45, "Almorox" },
					{ 621, 46, "Alboraya" },
					{ 622, 47, "Barcial de la Loma" },
					{ 623, 48, "Barakaldo" },
					{ 624, 49, "Argujillo" },
					{ 625, 50, "Alcalá de Ebro" },
					{ 626, 1, "Berantevilla" },
					{ 627, 2, "Ballestero, El" },
					{ 628, 3, "Alicante/Alacant" },
					{ 629, 4, "Almócita" },
					{ 630, 5, "Arenas de San Pedro" },
					{ 631, 6, "Azuaga" },
					{ 632, 7, "Capdepera" },
					{ 633, 8, "Aiguafreda" },
					{ 634, 9, "Altos, Los" },
					{ 635, 10, "Aldeanueva de la Vera" },
					{ 636, 11, "Conil de la Frontera" },
					{ 637, 12, "Ares del Maestrat" },
					{ 638, 13, "Almedina" },
					{ 639, 14, "Cañete de las Torres" },
					{ 640, 15, "Cabana de Bergantiños" },
					{ 641, 16, "Aliaguilla" },
					{ 642, 17, "Vajol, La" },
					{ 643, 18, "Alhendín" },
					{ 644, 20, "Asteasu" },
					{ 645, 21, "Bonares" },
					{ 646, 22, "Alcalá de Gurrea" },
					{ 647, 23, "Begíjar" },
					{ 648, 24, "Bembibre" },
					{ 649, 25, "Alfés" },
					{ 650, 26, "Anguiano" },
					{ 651, 27, "Corgo, O" },
					{ 652, 28, "Arganda del Rey" },
					{ 653, 29, "Alpandeire" },
					{ 654, 30, "Campos del Río" },
					{ 655, 31, "Ancín/Antzin" },
					{ 656, 32, "Bola, A" },
					{ 657, 33, "Carreño" },
					{ 658, 35, "Oliva, La" },
					{ 659, 36, "Crecente" },
					{ 660, 37, "Aldeadávila de la Ribera" },
					{ 661, 38, "Fuencaliente de la Palma" },
					{ 662, 39, "Cabuérniga" },
					{ 663, 40, "Aldehorno" },
					{ 664, 41, "Badolatosa" },
					{ 665, 42, "Aldehuelas, Las" },
					{ 666, 43, "Amposta" },
					{ 667, 44, "Alcorisa" },
					{ 668, 45, "Añover de Tajo" },
					{ 669, 46, "Albuixech" },
					{ 670, 47, "Barruelo del Valle" },
					{ 671, 48, "Barrika" },
					{ 672, 49, "Arquillinos" },
					{ 673, 50, "Alcalá de Moncayo" },
					{ 674, 2, "Barrax" },
					{ 675, 3, "Almoradí" },
					{ 676, 4, "Alsodux" },
					{ 677, 5, "Arevalillo" },
					{ 678, 6, "Badajoz" },
					{ 679, 7, "Ciutadella de Menorca" },
					{ 680, 8, "Badalona" },
					{ 681, 10, "Aldeanueva del Camino" },
					{ 682, 11, "Chiclana de la Frontera" },
					{ 683, 12, "Argelita" },
					{ 684, 13, "Almodóvar del Campo" },
					{ 685, 14, "Carcabuey" },
					{ 686, 15, "Cabanas" },
					{ 687, 16, "Almarcha, La" },
					{ 688, 17, "Banyoles" },
					{ 689, 18, "Alicún de Ortega" },
					{ 690, 19, "Aldeanueva de Guadalajara" },
					{ 691, 20, "Ataun" },
					{ 692, 21, "Cabezas Rubias" },
					{ 693, 22, "Alcalá del Obispo" },
					{ 694, 23, "Bélmez de la Moraleda" },
					{ 695, 24, "Benavides" },
					{ 696, 25, "Algerri" },
					{ 697, 26, "Arenzana de Abajo" },
					{ 698, 27, "Cospeito" },
					{ 699, 28, "Arroyomolinos" },
					{ 700, 29, "Antequera" },
					{ 701, 30, "Caravaca de la Cruz" },
					{ 702, 31, "Andosilla" },
					{ 703, 32, "Bolo, O" },
					{ 704, 33, "Caso" },
					{ 705, 34, "Arconada" },
					{ 706, 35, "Pájara" },
					{ 707, 36, "Cuntis" },
					{ 708, 37, "Aldea del Obispo" },
					{ 709, 38, "Garachico" },
					{ 710, 39, "Camaleño" },
					{ 711, 40, "Aldehuela del Codonal" },
					{ 712, 41, "Benacazón" },
					{ 713, 42, "Alentisque" },
					{ 714, 43, "Arbolí" },
					{ 715, 45, "Arcicóllar" },
					{ 716, 46, "Alcàsser" },
					{ 717, 47, "Becilla de Valderaduey" },
					{ 718, 48, "Basauri" },
					{ 719, 49, "Arrabalde" },
					{ 720, 50, "Alconchel de Ariza" },
					{ 721, 1, "Bernedo" },
					{ 722, 2, "Bienservida" },
					{ 723, 3, "Almudaina" },
					{ 724, 4, "Antas" },
					{ 725, 5, "Arévalo" },
					{ 726, 6, "Barcarrota" },
					{ 727, 7, "Consell" },
					{ 728, 8, "Bagà" },
					{ 729, 9, "Ameyugo" },
					{ 730, 10, "Aldehuela de Jerte" },
					{ 731, 11, "Chipiona" },
					{ 732, 12, "Artana" },
					{ 733, 13, "Almuradiel" },
					{ 734, 14, "Cardeña" },
					{ 735, 15, "Camariñas" },
					{ 736, 16, "Almendros" },
					{ 737, 17, "Bàscara" },
					{ 738, 18, "Almegíjar" },
					{ 739, 19, "Algar de Mesa" },
					{ 740, 20, "Aia" },
					{ 741, 21, "Cala" },
					{ 742, 22, "Alcampell" },
					{ 743, 23, "Benatae" },
					{ 744, 24, "Benuza" },
					{ 745, 25, "Alguaire" },
					{ 746, 26, "Arenzana de Arriba" },
					{ 747, 27, "Chantada" },
					{ 748, 28, "Atazar, El" },
					{ 749, 29, "Árchez" },
					{ 750, 30, "Cartagena" },
					{ 751, 31, "Ansoáin/Antsoain" },
					{ 752, 32, "Calvos de Randín" },
					{ 753, 33, "Castrillón" },
					{ 754, 35, "Palmas de Gran Canaria, Las" },
					{ 755, 36, "Dozón" },
					{ 756, 37, "Aldealengua" },
					{ 757, 38, "Garafía" },
					{ 758, 39, "Camargo" },
					{ 759, 40, "Aldeonte" },
					{ 760, 41, "Bollullos de la Mitación" },
					{ 761, 42, "Aliud" },
					{ 762, 43, "Arboç, L\'" },
					{ 763, 44, "Alfambra" },
					{ 764, 45, "Argés" },
					{ 765, 46, "Alcàntera de Xúquer" },
					{ 766, 47, "Benafarces" },
					{ 767, 48, "Berango" },
					{ 768, 49, "Aspariegos" },
					{ 769, 50, "Aldehuela de Liestos" },
					{ 770, 1, "Campezo/Kanpezu" },
					{ 771, 2, "Bogarra" },
					{ 772, 3, "Alqueria d\'Asnar, l\'" },
					{ 773, 4, "Arboleas" },
					{ 774, 5, "Aveinte" },
					{ 775, 6, "Baterno" },
					{ 776, 7, "Costitx" },
					{ 777, 8, "Balenyà" },
					{ 778, 9, "Anguix" },
					{ 779, 10, "Alía" },
					{ 780, 11, "Espera" },
					{ 781, 12, "Ayódar" },
					{ 782, 13, "Anchuras" },
					{ 783, 14, "Carlota, La" },
					{ 784, 15, "Cambre" },
					{ 785, 16, "Almodóvar del Pinar" },
					{ 786, 18, "Almuñécar" },
					{ 787, 19, "Algora" },
					{ 788, 20, "Azkoitia" },
					{ 789, 21, "Calañas" },
					{ 790, 22, "Alcolea de Cinca" },
					{ 791, 23, "Cabra del Santo Cristo" },
					{ 792, 24, "Bercianos del Páramo" },
					{ 793, 25, "Alins" },
					{ 794, 26, "Arnedillo" },
					{ 795, 27, "Folgoso do Courel" },
					{ 796, 28, "Batres" },
					{ 797, 29, "Archidona" },
					{ 798, 30, "Cehegín" },
					{ 799, 31, "Anue" },
					{ 800, 32, "Carballeda de Valdeorras" },
					{ 801, 33, "Castropol" },
					{ 802, 34, "Astudillo" },
					{ 803, 35, "Puerto del Rosario" },
					{ 804, 36, "Estrada, A" },
					{ 805, 37, "Aldeanueva de Figueroa" },
					{ 806, 38, "Granadilla de Abona" },
					{ 807, 39, "Campoo de Yuso" },
					{ 808, 40, "Anaya" },
					{ 809, 41, "Bormujos" },
					{ 810, 42, "Almajano" },
					{ 811, 43, "Argentera, L\'" },
					{ 812, 44, "Aliaga" },
					{ 813, 45, "Azután" },
					{ 814, 46, "Alzira" },
					{ 815, 47, "Bercero" },
					{ 816, 48, "Bermeo" },
					{ 817, 49, "Asturianos" },
					{ 818, 50, "Alfajarín" },
					{ 819, 1, "Zigoitia" },
					{ 820, 2, "Bonete" },
					{ 821, 3, "Altea" },
					{ 822, 4, "Armuña de Almanzora" },
					{ 823, 5, "Avellaneda" },
					{ 824, 6, "Benquerencia de la Serena" },
					{ 825, 7, "Deyá" },
					{ 826, 8, "Balsareny" },
					{ 827, 9, "Aranda de Duero" },
					{ 828, 10, "Aliseda" },
					{ 829, 11, "Gastor, El" },
					{ 830, 12, "Azuébar" },
					{ 831, 13, "Arenas de San Juan" },
					{ 832, 14, "Carpio, El" },
					{ 833, 15, "Capela, A" },
					{ 834, 16, "Almonacid del Marquesado" },
					{ 835, 17, "Bellcaire d\'Empordà" },
					{ 836, 18, "Alquife" },
					{ 837, 19, "Alhóndiga" },
					{ 838, 20, "Azpeitia" },
					{ 839, 21, "Campillo, El" },
					{ 840, 22, "Alcubierre" },
					{ 841, 23, "Cambil" },
					{ 842, 24, "Bercianos del Real Camino" },
					{ 843, 26, "Arnedo" },
					{ 844, 27, "Fonsagrada, A" },
					{ 845, 28, "Becerril de la Sierra" },
					{ 846, 29, "Ardales" },
					{ 847, 30, "Ceutí" },
					{ 848, 31, "Añorbe" },
					{ 849, 32, "Carballeda de Avia" },
					{ 850, 33, "Coaña" },
					{ 851, 34, "Autilla del Pino" },
					{ 852, 35, "San Bartolomé" },
					{ 853, 36, "Forcarei" },
					{ 854, 37, "Aldeanueva de la Sierra" },
					{ 855, 38, "Guancha, La" },
					{ 856, 39, "Cartes" },
					{ 857, 40, "Añe" },
					{ 858, 41, "Brenes" },
					{ 859, 42, "Almaluez" },
					{ 860, 43, "Arnes" },
					{ 861, 44, "Almohaja" },
					{ 862, 45, "Barcience" },
					{ 863, 46, "Alcublas" },
					{ 864, 47, "Berceruelo" },
					{ 865, 48, "Berriatua" },
					{ 866, 49, "Ayoó de Vidriales" },
					{ 867, 50, "Alfamén" },
					{ 868, 1, "Kripan" },
					{ 869, 2, "Bonillo, El" },
					{ 870, 3, "Aspe" },
					{ 871, 4, "Bacares" },
					{ 872, 5, "Ávila" },
					{ 873, 6, "Berlanga" },
					{ 874, 7, "Escorca" },
					{ 875, 8, "Barcelona" },
					{ 876, 9, "Arandilla" },
					{ 877, 10, "Almaraz" },
					{ 878, 11, "Grazalema" },
					{ 879, 13, "Argamasilla de Alba" },
					{ 880, 14, "Castro del Río" },
					{ 881, 15, "Carballo" },
					{ 882, 16, "Altarejos" },
					{ 883, 17, "Besalú" },
					{ 884, 19, "Alique" },
					{ 885, 20, "Beasain" },
					{ 886, 21, "Campofrío" },
					{ 887, 22, "Alerre" },
					{ 888, 23, "Campillo de Arenas" },
					{ 889, 24, "Berlanga del Bierzo" },
					{ 890, 25, "Almacelles" },
					{ 891, 26, "Arrúbal" },
					{ 892, 27, "Foz" },
					{ 893, 28, "Belmonte de Tajo" },
					{ 894, 29, "Arenas" },
					{ 895, 30, "Cieza" },
					{ 896, 31, "Aoiz/Agoitz" },
					{ 897, 32, "Carballiño, O" },
					{ 898, 33, "Colunga" },
					{ 899, 34, "Autillo de Campos" },
					{ 900, 35, "San Bartolomé de Tirajana" },
					{ 901, 36, "Fornelos de Montes" },
					{ 902, 37, "Aldearrodrigo" },
					{ 903, 38, "Guía de Isora" },
					{ 904, 39, "Castañeda" },
					{ 905, 40, "Arahuetes" },
					{ 906, 41, "Burguillos" },
					{ 907, 42, "Almarza" },
					{ 908, 43, "Ascó" },
					{ 909, 44, "Alobras" },
					{ 910, 45, "Bargas" },
					{ 911, 46, "Alcúdia, l\'" },
					{ 912, 47, "Berrueces" },
					{ 913, 48, "Berriz" },
					{ 914, 49, "Barcial del Barco" },
					{ 915, 50, "Alforque" },
					{ 916, 1, "Kuartango" },
					{ 917, 2, "Carcelén" },
					{ 918, 3, "Balones" },
					{ 919, 4, "Bayárcal" },
					{ 920, 6, "Bienvenida" },
					{ 921, 7, "Esporles" },
					{ 922, 8, "Begues" },
					{ 923, 9, "Arauzo de Miel" },
					{ 924, 10, "Almoharín" },
					{ 925, 11, "Jerez de la Frontera" },
					{ 926, 12, "Barracas" },
					{ 927, 13, "Argamasilla de Calatrava" },
					{ 928, 14, "Conquista" },
					{ 929, 15, "Carnota" },
					{ 930, 16, "Arandilla del Arroyo" },
					{ 931, 17, "Bescanó" },
					{ 932, 18, "Arenas del Rey" },
					{ 933, 19, "Almadrones" },
					{ 934, 20, "Beizama" },
					{ 935, 21, "Cañaveral de León" },
					{ 936, 22, "Alfántega" },
					{ 937, 23, "Canena" },
					{ 938, 24, "Boca de Huérgano" },
					{ 939, 25, "Almatret" },
					{ 940, 26, "Ausejo" },
					{ 941, 27, "Friol" },
					{ 942, 28, "Berzosa del Lozoya" },
					{ 943, 29, "Arriate" },
					{ 944, 30, "Fortuna" },
					{ 945, 31, "Araitz" },
					{ 946, 32, "Cartelle" },
					{ 947, 33, "Corvera de Asturias" },
					{ 948, 34, "Ayuela" },
					{ 949, 35, "Aldea de San Nicolás, La" },
					{ 950, 36, "Agolada" },
					{ 951, 37, "Aldearrubia" },
					{ 952, 38, "Güímar" },
					{ 953, 39, "Castro-Urdiales" },
					{ 954, 40, "Arcones" },
					{ 955, 41, "Cabezas de San Juan, Las" },
					{ 956, 42, "Almazán" },
					{ 957, 43, "Banyeres del Penedès" },
					{ 958, 44, "Alpeñés" },
					{ 959, 45, "Belvís de la Jara" },
					{ 960, 46, "Alcúdia de Crespins, l\'" },
					{ 961, 47, "Bobadilla del Campo" },
					{ 962, 48, "Bilbao" },
					{ 963, 49, "Belver de los Montes" },
					{ 964, 50, "Alhama de Aragón" },
					{ 965, 1, "Elburgo/Burgelu" },
					{ 966, 2, "Casas de Juan Núñez" },
					{ 967, 3, "Banyeres de Mariola" },
					{ 968, 4, "Bayarque" },
					{ 969, 5, "Barco de Ávila, El" },
					{ 970, 6, "Bodonal de la Sierra" },
					{ 971, 7, "Estellencs" },
					{ 972, 8, "Bellprat" },
					{ 973, 9, "Arauzo de Salce" },
					{ 974, 10, "Arroyo de la Luz" },
					{ 975, 11, "Jimena de la Frontera" },
					{ 976, 12, "Betxí" },
					{ 977, 13, "Arroba de los Montes" },
					{ 978, 14, "Córdoba" },
					{ 979, 15, "Carral" },
					{ 980, 17, "Beuda" },
					{ 981, 18, "Armilla" },
					{ 982, 19, "Almoguera" },
					{ 983, 20, "Belauntza" },
					{ 984, 21, "Cartaya" },
					{ 985, 22, "Almudévar" },
					{ 986, 23, "Carboneros" },
					{ 987, 24, "Boñar" },
					{ 988, 25, "Almenar" },
					{ 989, 26, "Autol" },
					{ 990, 27, "Xermade" },
					{ 991, 28, "Berrueco, El" },
					{ 992, 29, "Atajate" },
					{ 993, 30, "Fuente Álamo de Murcia" },
					{ 994, 31, "Aranarache/Aranaratxe" },
					{ 995, 32, "Castrelo do Val" },
					{ 996, 33, "Cudillero" },
					{ 997, 35, "Santa Brígida" },
					{ 998, 36, "Gondomar" },
					{ 999, 37, "Aldeaseca de Alba" },
					{ 1000, 38, "Hermigua" }
				});

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "Organizaciones",
				columns: new[] { "Id", "Activa", "Nombre", "Observaciones", "FechaAlta" },
				values: new object[] { 1, true, "EAPN Madrid", null, new DateTime() });

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "Usuarios",
				columns: new[] { "Id", "Email", "Nombre", "Apellidos", "Hash", "IntentosLogin", "Activo", "OrganizacionId", "FechaAlta" },
				values: new object[,]
				{{1,"Jjavierrg@gmail.com","José Javier","Rodríguez Gallego","$2b$12$sn1hzFuoYJwKxocoCsn3me4gLtEeMG9sJrz/FQQu6XMww3bdoSgOe",0,true,1,"0001-01-01T00:00:00"},{2,"test@test.com","Test","Test","$2b$12$vrDFckbZnoXbB9oFeLqqn.UQtnQ2UdYOJC/r6UqrjLfS00LagnO0q",0,true,1,"0001-01-01T00:00:00"}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "UsuariosPerfiles",
			   columns: new[] { "UsuarioId", "PerfilId" },
			   values: new object[,]
			   {{2,1},{1,2}});

			migrationBuilder.InsertData(
			   schema: "dbo",
			   table: "UsuariosPermisos",
			   columns: new[] { "UsuarioId", "PermisoId" },
			   values: new object[,]
			   {{1,6}});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Adjuntos_Fichas_FichaId",
				schema: "dbo",
				table: "Adjuntos");

			migrationBuilder.DropTable(
				name: "Configuraciones",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "IndicadoresSeguimientos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Logs",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "PerfilesPermisos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Rangos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "UsuariosPerfiles",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "UsuariosPermisos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Indicadores",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Seguimientos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Perfiles",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Permisos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Categorias",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Dimensiones",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Fichas",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Sexos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Municipios",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Paises",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Empadronamientos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "SituacionesAdministrativas",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Usuarios",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Provincias",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Organizaciones",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Adjuntos",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "TiposAdjunto",
				schema: "dbo");
		}
	}
}
