using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EAPN.HDVS.Web.Migrations
{
    public partial class AddedAdjuntosAreasDimensionesandIndicadoresEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposAdjunto",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    Carpeta = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAdjunto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adjuntos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "Areas",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconoId = table.Column<int>(nullable: true),
                    Descripcion = table.Column<string>(maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Adjuntos_IconoId",
                        column: x => x.IconoId,
                        principalSchema: "dbo",
                        principalTable: "Adjuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dimensiones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dimensiones_Areas_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "dbo",
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicadores",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DimensionId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    Activo = table.Column<bool>(nullable: false),
                    Puntuacion = table.Column<int>(nullable: false),
                    Sugerencias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicadores_Dimensiones_DimensionId",
                        column: x => x.DimensionId,
                        principalSchema: "dbo",
                        principalTable: "Dimensiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicadorFichas",
                schema: "dbo",
                columns: table => new
                {
                    IndicadorId = table.Column<int>(nullable: false),
                    FichaId = table.Column<int>(nullable: false),
                    Observaciones = table.Column<string>(nullable: true)
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
                name: "IX_Areas_IconoId",
                schema: "dbo",
                table: "Areas",
                column: "IconoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dimensiones_AreaId",
                schema: "dbo",
                table: "Dimensiones",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Indicadores_DimensionId",
                schema: "dbo",
                table: "Indicadores",
                column: "DimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorFichas_IndicadorId",
                schema: "dbo",
                table: "IndicadorFichas",
                column: "IndicadorId");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "TiposAdjunto",
                columns: new[] { "Id", "Descripcion", "Carpeta" },
                values: new object[,]
                {
                    { 1, "Imagenes", "images"},
                    { 2, "Adjuntos", "attachments" },
                    { 3, "Documentacion", "docs" },
                    { 4, "Personales Ficha", "personal" },
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Areas",
                columns: new[] { "Id", "IconoId", "Descripcion", "Activo" },
                values: new object[,]
                {
                    { 1, null, "Vivienda", true},
                    { 2, null, "Salud", true },
                    { 3, null, "Factores Estructurales", true },
                    { 4, null, "Habilidades personales y sociales", true },
                    { 5, null, "Derechos Civiles y Políticos", true },
                    { 6, null, "Situación económica", true },
                    { 7, null, "Situación educativa y formativa", true },
                    { 8, null, "Situación Ocupacional", true },
                    { 9, null, "Situación Personal y Familiar", true },
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Dimensiones",
                columns: new[] { "Id", "AreaId", "Descripcion", "Activo" },
                values: new object[,]
                {
                    { 1, 1, "Vivienda Segura", true },
                    { 2, 1, "Vivienda Insegura", true },
                    { 3, 1, "Vivienda Inadecuada", true },
                    { 4, 1, "Sin Vivienda ", true },
                    { 5, 1, "Sin Techo", true },
                    { 6, 2, "Adicciones", true },
                    { 7, 2, "Salud física", true },
                    { 8, 2, "Salud mental", true },
                    { 9, 2, "Adherencia a tratamiento médico", true },
                    { 10, 2, "Discapacidad, Dependencia e Incapacidad", true },
                    { 11, 2, "Acceso a atención sanitaria", true },
                    { 12, 3, "Factores Ambientales", true },
                    { 13, 3, "Factores Institucionales", true },
                    { 14, 4, "Habilidades personales y sociales", true },
                    { 15, 4, "Competencias cognitivas básicas", true },
                    { 16, 4, "Competencias Instrumentales", true },
                    { 17, 5, "Relación con el sistema penal", true },
                    { 18, 5, "Extranjería", true },
                    { 19, 5, "Discriminación por identidad de género o sexual ", true },
                    { 20, 5, "Discriminación por raza o etnia", true },
                    { 21, 5, "Discriminación por adscripción religiosa", true },
                    { 22, 5, "Discriminación por situación de salud (VIH, discapacidad, etc.)", true },
                    { 23, 5, "Empadronamiento", true },
                    { 24, 6, "Carencia o insuficiencia de ingresos", true },
                    { 25, 6, "Privación material del hogar", true },
                    { 26, 6, "Nivel de endeudamiento ", true },
                    { 27, 6, "Tipo de deuda", true },
                    { 28, 6, "Fuentes de ingresos", true },
                    { 29, 7, "Escolarización de menores", true },
                    { 30, 7, "Nivel de Estudios", true },
                    { 31, 7, "Competencias", true },
                    { 32, 7, "Homologación de estudios para personas extranjeras", true },
                    { 33, 8, "Empleabilidad", true },
                    { 34, 8, "Desempleados/as parados/as", true },
                    { 35, 8, "Ocupados/as", true },
                    { 36, 9, "Edad", true },
                    { 37, 9, "Tutela", true },
                    { 38, 9, "Violencia de género", true },
                    { 39, 9, "Maltrato infantil", true },
                    { 40, 9, "Maltrato a mayores", true },
                    { 41, 9, "Responsabilidades familiares", true },
                    { 42, 9, "Hacinamiento ", true },
                    { 43, 9, "Convivencia", true },
                    { 44, 9, "Apoyo social y/o familiar", true }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Indicadores",
                columns: new[] { "Id", "DimensionId", "Descripcion", "Activo", "Puntuacion", "Sugerencias" },
                values: new object[,]
                {
                    { 1, 1, "1. Vivienda o alojamiento propio", true, 0, "Certificado del registro de la propiedad, y/o escritura" },
                    { 2, 1, "2. Vivienda cedida", true, 0, "En caso que se considere necesario, declaración jurada de propietario/a con fotocopia simple de escritura de la propiedad" },
                    { 3, 1, "3. Vivienda o habitación alquilada  con contrato", true, 0, "Certificado de empadronamiento, fotocopia de contrato de alquiler" },
                    { 4, 1, "4. Persona institucionalizada, por ejemplo, en residencias con plaza asignada", true, 0, "" },
                    { 5, 1, "5. Vivienda o alojamiento de la familia de origen", true, 0, "" },
                    { 6, 2, "1. Personas viviendo en un régimen de tenencia inseguro, sin pagar el alquiler", true, 4, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo)" },
                    { 7, 2, "2. Personas viviendo bajo amenaza de desahucio ", true, 4, "Notificación legal de abandono de la vivienda, orden legal de desahucio, orden de devolución vivienda en propiedad" },
                    { 8, 2, "3. Personas viviendo en asentamiento marginal", true, 4, "Certificado de convivencia" },
                    { 9, 2, "4. Personas viviendo en caravanas", true, 4, "Certificado de convivencia" },
                    { 10, 2, "5. Vivienda ocupada", true, 4, "Certificado de convivencia" },
                    { 11, 2, "6. Persona/as que subarriendan vivienda o habitación sin contrato, o residen en vivienda cedida, de forma irregular y sin contrato", true, 3, "Valoración de trabajador/a social" },
                    { 12, 3, "1. Vive en un edificio con barreras arquitectónicas", true, 1, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo)" },
                    { 13, 3, "2. Son alojamientos inadecuados que no contienen servicios indispensables para la salud, la seguridad, la comodidad y la nutrición", true, 3, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo)" },
                    { 14, 4, "1. Vive en un albergue o centro para personas sin hogar de larga estancia", true, 4, "Certificado del recurso de alojamiento" },
                    { 15, 4, "2. Vive en un alojamiento temporal para mujeres (de emergencia) ", true, 4, "Certificado del recurso de alojamiento" },
                    { 16, 4, "3. Personas en centros de acogida para solicitantes de protección internacional e inmigrantes", true, 4, "Certificado del recurso de alojamiento" },
                    { 17, 4, "4. Personas que en un plazo definido van a ser despedidas de instituciones residenciales o de internamiento", true, 4, "Certificado del recurso de alojamiento" },
                    { 18, 4, "5. Personas que reciben alojamiento con apoyo sostenido debido a su condición de personas sin hogar", true, 4, "Certificado del recurso de alojamiento" },
                    { 19, 5, "1. Sin techo. Vive en espacios públicos (incluye dormir en coches)", true, 5, "Informe del Samur Social" },
                    { 20, 5, "2. Pernocta  en un albergue de corta estancia", true, 5, "Certificado del recurso de alojamiento" },
                    { 21, 5, "3. Itinerarios de calle. Alterna vivir en la calle y otros tipos de situaciones de alojamiento como alojamiento en infravivienda, pensiones, alquiler de habitaciones, estancias en alojamientos institucionales, viviendas de amistades y conocidos", true, 5, "" },
                    { 22, 6, "1. Dependencia de alcohol/ u otras adicciones SIN tratamiento", true, 5, "Informe médico, informe de Servicios Especializados (CAD, CAID, etc.), informe de organización social" },
                    { 23, 6, "2. Dependencia de alcohol/ u otras adicciones CON tratamiento", true, 3, "Informe médico, informe de Servicios Especializados (CAD, CAID, etc.) " },
                    { 24, 6, "3. Abuso: consumo importante, que no llega a dependencia, sin tratamiento", true, 2, "" },
                    { 25, 6, "4. Antecedentes de consumo, sin consumo activo en el presente", true, 1, "" },
                    { 26, 6, "5. Sin adicciones", true, 0, "" },
                    { 27, 7, "1. Enfermedades graves degenerativas y crónicas terminales, que afecten gravemente el desarrollo de la vida cotidiana", true, 5, "Informe médico" },
                    { 28, 7, "2. Deterioro grave de la salud (enfermedades graves, desnutrición, enfermedades infecciosas, enfermedades que provocan dependencia) que afecte gravemente al desarrollo de la vida cotidiana y precise tratamiento continuado, supervisión y seguimiento", true, 4, "Informe médico" },
                    { 29, 7, "3. Enfermedades crónicas no degenerativas que requieren supervisión, tratamiento y/o medicación muy frecuente", true, 3, "Informe médico" },
                    { 30, 8, "1. Enfermedades crónicas degenerativas que afectan al desarrollo de su vida cotidiana", true, 4, "Informe médico" },
                    { 31, 8, "2. Enfermedades crónicas no degenerativas que requieren supervisión, tratamiento y/o medicación muy frecuente", true, 3, "Informe médico" },
                    { 32, 8, "3. Indicios de problemas de salud mental y/o deterioro cognitivo sin diagnosticar", true, 1, "" },
                    { 33, 9, "1. Sí adhiere a tratamiento", true, 0, "" },
                    { 34, 9, "2. No adhiere a tratamiento", true, 1, "" },
                    { 35, 10, "1. Discapacidad reconocida, del 33% a 64%", true, 2, "Certificado o carné de discapacidad" },
                    { 36, 10, "2. Discapacidad reconocida, 65% o superior", true, 4, "" },
                    { 37, 10, "3. Dependencia Grado I", true, 2, "Resolución del organismo competente" },
                    { 38, 10, "4. Dependencia Grado II", true, 3, "Resolución del organismo competente" },
                    { 39, 10, "5. Dependencia Grado III", true, 4, "Resolución del organismo competente" },
                    { 40, 10, "6. Incapacidad permanente parcial para la profesión habitual", true, 1, "Resolución del INSS" },
                    { 41, 10, "7. Incapacidad permanente total para la profesión habitual", true, 2, "Resolución del INSS" },
                    { 42, 10, "8. Incapacidad permanente absoluta para todo trabajo", true, 3, "Resolución del INSS" },
                    { 43, 10, "9. Gran invalidez", true, 4, "Resolución del INSS" },
                    { 44, 11, "1. Sin acceso a atención sanitaria", true, 2, "Confirmación de denegación de la tarjeta sanitaria (Tesorería de la Seguridad Social) " },
                    { 45, 11, "2. Con acceso a atención sanitaria", true, 0, "" },
                    { 46, 12, "1. Vive en un territorio fuertemente estigmatizado (inseguridad ciudadana como tráfico de drogas por bandas, prostitución, alta tasa de delitos violentos como homicidios, falta de presencia policial, ausencia de cohesión social, etc.)", true, 2, "Valoración del trabajador/a social, Denuncia vecinal y/o de entidades sociales. Presencia en medios de comunicación, prensa, radio" },
                    { 47, 12, "2. Vive en un entorno degradado ambiental, económica y socialmente, con núcleos abandonados, insuficiencia de servicios básicos, mal estado o inexistencia de parques y jardines, edificios públicos sin uso, deficiencia en el transporte, alto precio de servicios y productos básicos", true, 2, "Valoración del trabajador/a social, documentación técnica o estudios territoriales. Está considerado en Plan de Barrio" },
                    { 48, 12, "3. Vive en un área o región al margen del dinamismo social del resto de las regiones: altas tasas de paro, de paro de larga duración, de hogares con todos sus miembros en paro, ausencia del pequeño comercio, etc.", true, 1, "Valoración del trabajador/a social, documentación técnica o estudios territoriales " },
                    { 49, 13, "1. Existen limitaciones o dificultades en la respuesta de las instituciones a las necesidades de la persona. Cumple requisitos, pero no recibe respuesta satisfactoria en el acceso a servicios, recursos, o prestaciones económicas por carestía, discriminación o desconocimiento", true, 2, "Valoración del trabajador/a social, informes o estudios técnicos sobre planificación de recursos" },
                    { 50, 13, "2. Ausencia de dificultades", true, 0, "" },
                    { 51, 14, "1. Dificultades en autoestima - autoconcepto", true, 3, "Valoración del trabajador/a social, valoración del Equipo de Orientación Educativa" },
                    { 52, 14, "2. Dificultades en sus interacciones interpersonales", true, 2, "Valoración del trabajador/a social" },
                    { 53, 14, "3. Dificultades en cumplimiento de asistencia/responsabilidad/ puntualidad, en la comprensión y aceptación de indicaciones, normas y procedimientos", true, 1, "Valoración del trabajador/a social" },
                    { 54, 15, "1. Dificultades en comprensión y expresión del lenguaje, en la capacidad de memoria (corto, medio y largo plazo), en la capacidad para mantener la atención y concentración, Habilidades de auto observación y determinación de necesidades propias y/o de terceros, Capacidad para analizar la realidad, dificultad para interpretar las causas de lo que ocurre a su alrededor y lo que le ocurre a uno mismo. Dificultad para pararse a pensar y calcular las posibles consecuencias que tendrán sus comportamientos para la propia persona y las demás", true, 3, "Valoración del trabajador/a social" },
                    { 55, 15, "2. Sin dificultades en competencias cognitivas básicas", true, 0, "" },
                    { 56, 16, "1. Dificultades en las habilidades para hacer y/o mantener amistades, para usar los recursos comunitarios, seguir prescripciones de salud, para participar y organizarse en la vida comunitaria, para organizar el tiempo libre, para el auto cuidado (higiene, ropa, medicación, etc.), para la convivencia con otras personas, para tomar decisiones autónomamente, para afrontar crisis vitales, incapacidad de organización y planificación en la utilización  de sistemas de protección social y en el ejercicio de derechos. No manejo del tiempo", true, 1, "Valoración del trabajador/a social" },
                    { 57, 16, "2. Dificultades para encontrar un sitio donde vivir, un empleo  y/o mantenerlo", true, 3, "Valoración del trabajador/a social" },
                    { 58, 17, "1. Reclusos/as", true, 5, "Sentencias judiciales, certificado de Instituciones Penitenciarias, y/o Auto de puesta en libertad" },
                    { 59, 17, "2. Ex reclusos/as con más de 10 años en prisión", true, 5, "Sentencias judiciales, certificado de instituciones penitenciarias, y/o Auto de puesta en libertad, informe de instituciones de tratamiento, informe de reclamación por errores judiciales" },
                    { 60, 17, "3. Ex reclusos/as con menos de 10 años", true, 4, "Sentencias judiciales, certificado de instituciones penitenciarias, y/o Auto de puesta en libertad, informe de instituciones de tratamiento, informe de reclamación por errores judiciales" },
                    { 61, 17, "4. Causas judiciales pendientes ", true, 1, "Sentencias judiciales, certificados de antecedentes penales" },
                    { 62, 17, "5. Causas judiciales, con antecedentes, sin ingreso en prisión y situación de libertad condicional", true, 2, "" },
                    { 63, 18, "1. Extranjeros/as con autorización de residencia y trabajo", true, 0, "" },
                    { 64, 18, "2. Extranjeros/as sin autorización de residencia", true, 4, "NIE caducado, en caso de estimarse necesario, órdenes de expulsión o certificados de ONG's especializadas colaboradoras" },
                    { 65, 18, "3. Extranjeros/as sin autorización de trabajo", true, 2, "Autorización de residencia excluyendo la autorización de trabajo, o autorización de residencia acompañada de informe de ONG's especializada colaboradora explicando dicha situación. Resolución aprobada con autorización de residencia" },
                    { 66, 18, "4. Solicitantes de protección internacional", true, 2, "Tarjeta de solicitante de protección internacional" },
                    { 67, 18, "5. Solicitantes de apatridia", true, 3, "Tarjeta de solicitante de Apatridia, resolución del Ministerio de Interior de la concesión del Estatuto de Apátrida, documento de identidad de apátrida (tarjeta NIE)" },
                    { 68, 18, "6. Personas bajo protección internacional", true, 2, "Resolución del Ministerio de Interior de la concesión del Estatuto de Refugiado/a o Protección subsidiaria , documento de identidad (tarjeta NIE)" },
                    { 69, 19, "1. Personas transexuales, transgénero o transvestidas que hayan sufrido discriminación  debido a su identidad de género, sin documentación identificativa y/o administrativa acorde a su identidad de género", true, 5, "Informes médicos de su proceso de transexualización. Informes o certificados de ONG's especializadas colaboradoras" },
                    { 70, 19, "2. Personas transexuales, transgénero o transvestidas que hayan sufrido situaciones de discriminación debido a su identidad de género, CON DNI regularizado", true, 3, "Informes médicos de su proceso de transexualización. Informes o certificados de ONG's especializadas colaboradoras" },
                    { 71, 19, "3. Personas que hayan sufrido situaciones de discriminación debido a su orientación sexual (bisexuales, gays,  lesbianas, etc.)", true, 3, "Cartas de despido, reclamaciones o denuncias ante el Servicio de Mediación y Arbitraje o los Juzgados de lo Social, informes de ONG's especializadas colaboradoras, informes policiales" },
                    { 72, 20, "1. Personas que hayan sufrido discriminación debido a su raza o etnia", true, 2, "Cartas de despido, reclamaciones o denuncias ante el Servicio de Mediación y Arbitraje o los Juzgados de lo Social, informes de ONG's especializadas colaboradoras" },
                    { 73, 21, "1. Personas que hayan sufrido discriminación debido a su adscripción religiosa", true, 1, "Cartas de despido, reclamaciones o denuncias ante el Servicio de Mediación y Arbitraje o los Juzgados de lo Social, informes de ONG's " },
                    { 74, 22, "1. Personas que hayan sufrido situaciones de discriminación debido a su situación de salud", true, 1, "" },
                    { 75, 23, "1. Esta empadronada/o en el lugar de residencia", true, 0, "Volante de empadronamiento" },
                    { 76, 23, "2. Está empadronado/a en el mismo municipio en el que reside, pero no vive en el domicilio del padrón", true, 0, "" },
                    { 77, 23, "3. Está empadronado/a en el mismo municipio en el que reside, pero no vive donde consta empadronado/a ni tiene posibilidades de regularizar su situación en el padrón municipal", true, 2, "" },
                    { 78, 23, "4. Está empadronado/a en un municipio distinto que el de residencia, y tiene dificultades para regularizar su situación en el padrón municipal de residencia", true, 2, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo, declaración jurada" },
                    { 79, 23, "5. No está empadronado/a, pero podría regularizar su situación fácilmente", true, 1, "Volante de empadronamiento, Histórico de padrón, declaración jurada, informe de entidades colaboradoras, informe certificado de convivencia" },
                    { 80, 23, "6. No está empadronado/a, y tiene dificultades para hacerlo", true, 2, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo, declaración jurada, informe de entidades colaboradoras, informe certificado de convivencia" },
                    { 81, 24, "1. Carencia o insuficiencia de ingresos grave. Ingresos mensuales igual o inferiores a 450 euros para la primera persona del hogar, y 200 a partir de la segunda. Por ejemplo: Para 2 personas, igual o inferiores a 650 euros. Para 3 personas, igual o inferiores a 850 euros. Para 4 personas, igual o inferiores a 1050 euros, etc.", true, 5, "Declaración de la Renta, Certificado de Hacienda y/o Seguridad Social. Declaración jurada de ingresos, notificación de concesión de prestación social, movimientos de cuenta bancaria, contrato de trabajo, certificado de intereses bancarios" },
                    { 82, 24, "2. Carencia o insuficiencia de ingresos moderada. Ingresos mensuales igual o inferiores a 550 euros para la primera persona del hogar, y 200 a partir de la segunda. Por ejemplo: Para 2 personas, igual o inferiores a 750 euros. Para 3 personas, igual o inferiores a 950 euros. Para 4 personas, igual o inferiores a 1150 euros, etc.", true, 4, "Declaración de la Renta, Certificado de Hacienda y/o Seguridad Social. Declaración jurada de ingresos, notificación de concesión de prestación social, movimientos de cuenta bancaria, contrato de trabajo, certificado de intereses bancarios" },
                    { 83, 24, "3. Carencia o insuficiencia de ingresos leve. Ingresos mensuales igual o inferiores a 650 euros para la primera persona del hogar, y 200 a partir de la segunda. Por ejemplo: Para 2 personas, igual o inferiores a 850 euros. Para 3 personas, igual o inferiores a 1050 euros. Para 4 personas, igual o inferiores a 1250 euros, etc.", true, 3, "Declaración de la Renta, Certificado de Hacienda y/o Seguridad Social. Declaración jurada de ingresos, notificación de concesión de prestación social, movimientos de cuenta bancaria, contrato de trabajo, certificado de intereses bancarios" },
                    { 84, 24, "4. Sin problemas de ingresos", true, 0, "" },
                    { 85, 25, "1. Con privación material (cumple con 4 de los 9 items)", true, 5, "Valoración del trabajador/a social (se sugiere fotocopia de cartilla del banco, recibo o letras devueltas, reconocimiento de deuda con el banco, etc.)" },
                    { 86, 25, "2. Sin privación material severa", true, 0, "" },
                    { 87, 26, "1. Grave: deudas por más de 2 veces de los ingresos familiares ", true, 5, "Recibos o letras devueltas, documentación de reconocimiento de deuda con banco, empresas suministradoras de luz, gas, agua. Certificado de no estar al corriente de pago con la seguridad social y/o la agencia tributaria. Fraccionamientos de pago con hacienda o seguridad social adeudados" },
                    { 88, 26, "2. Medio: deudas por menos de 2 veces de los ingresos anuales familiares ", true, 3, "Recibos o letras devueltas, documentación de reconocimiento de deuda con banco, empresas suministradoras de luz, gas, agua. Certificado de no estar al corriente de pago con la seguridad social y/o la agencia tributaria. Fraccionamientos de pago con hacienda o seguridad social adeudados" },
                    { 89, 26, "3. Nivel de endeudamiento leve o sin nivel de endeudamiento", true, 0, "" },
                    { 90, 27, "1. Hipotecaria, con la Seguridad Social y/o administraciones públicas", true, 5, "Recibos o letras devueltas, documentación de reconocimiento de deuda con banco, empresas suministradoras de luz, gas, agua. Certificado de no estar al corriente de pago con la seguridad social y/o la agencia tributaria. Fraccionamientos de pago con hacienda o seguridad social adeudados" },
                    { 91, 27, "2. Otro tipo de deudas ", true, 0, "" },
                    { 92, 28, "1. Los  ingresos dependen principalmente del apoyo de la red primaria o de economía sumergida", true, 1, "Declaración jurada de ingresos, movimientos de cuenta bancaria" },
                    { 93, 28, "2. Los ingresos dependen principalmente de pensiones contributivas de jubilación, viudedad, orfandad, incapacidad permanente o invalidez", true, 0, "Declaración jurada de ingresos, movimientos de cuenta bancaria. Certificado acreditativo de la seguridad social" },
                    { 94, 28, "3. Los  ingresos dependen principalmente de prestaciones sociales con una duración igual o superior a un año", true, 0, "Declaración jurada de ingresos, movimientos de cuenta bancaria. Certificado del portal de empleo o de la oficina de empleo" },
                    { 95, 28, "4. Los  ingresos dependen principalmente de prestaciones sociales con una duración inferior a un año", true, 1, "Declaración jurada de ingresos, movimientos de cuenta bancaria. Resolución positiva de la solicitud de RMI o PNC por parte de la Consejería" },
                    { 96, 28, "5. Los  ingresos dependen principalmente de ayudas sociales puntuales", true, 2, "Certificado de administración u organización responsable de la ayuda" },
                    { 97, 28, "6. Los Ingresos proceden principalmente de su trabajo", true, 0, "Nómina, transferencia bancaria o recibo de ingreso" },
                    { 98, 29, "1. Hogar con menores sin escolarizar, en edad de escolarización obligatoria", true, 5, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada" },
                    { 99, 30, "1. No lee ni escribe", true, 5, "Valoración del trabajador/a social " },
                    { 100, 30, "2. Sin estudios (sólo lee y escribe)", true, 5, "Valoración del trabajador/a social " },
                    { 101, 30, "3. Estudios primarios incompletos", true, 4, "Valoración del trabajador/a social " },
                    { 102, 30, "4. Educación Primaria/Enseñanzas elementales de música y danza/Antiguo Graduado Escolar/Etapa Educación Secundaria Obligatoria (ESO)/Certificado de cualificación profesional nivel 1", true, 3, "Certificado de estudios realizados" },
                    { 103, 30, "5. Escuelas Taller y Casas de oficio", true, 2, "Certificado de estudios realizados" },
                    { 104, 30, "6. Ciclos formativos de Formación Profesional Básica/Programas de Garantía Social /PCPI ", true, 3, "Certificado de estudios realizados" },
                    { 105, 30, "7. Certificados de cualificación profesional nivel 2", true, 2, "Certificado de estudios realizados" },
                    { 106, 30, "8. Bachillerato/Antiguo COU y BUP/Ciclos Formativos de Grado medio de F.P./Enseñanzas Deportivas de Grado medio/Ciclos formativos de Grado Medio de Artes Plásticas y Diseño/Antigua Formación Profesional PF1", true, 1, "Certificado de estudios realizados" },
                    { 107, 30, "9. Certificados de cualificación profesional nivel 3", true, 0, "Certificado de estudios realizados" },
                    { 108, 30, "10. Ciclos Formativos de Grado Superior de F.P./ Enseñanzas Deportivas de Grado Superior/Ciclos Formativos de Grado Superior de Artes Plásticas y Diseño/Antigua Formación Profesional FP2", true, 0, "Certificado de estudios realizados" },
                    { 109, 30, "11. Estudios Universitarios: Estudios de Grado/Estudios de Máster/Estudios de Doctorado", true, 0, "Certificado de estudios realizados" },
                    { 110, 30, "12. Antigua Diplomatura Universitaria, Licenciatura Universitaria", true, 0, "Certificado de estudios realizados" },
                    { 111, 31, "1. Dificultades en la comunicación oral en lengua local", true, 5, "Valoración del trabajador/a social a través de entrevista personal" },
                    { 112, 31, "2. Dificultades en la comunicación escrita en lengua local", true, 4, "Valoración del trabajador/a social a través de entrevista personal" },
                    { 113, 31, "3. Carencias o dificultades en competencias matemáticas (uso y desarrollo de razonamientos matemáticos)", true, 2, "Valoración del trabajador/a social a través de entrevista personal, valoración del Equipo de Orientación Educativa" },
                    { 114, 31, "4. Carencias o dificultades en el conocimiento y manejo del entorno", true, 2, "Valoración del trabajador/a social a través de entrevista personal" },
                    { 115, 31, "5. Carencias o dificultades en competencia digital (familiaridad y uso de las TIC)", true, 1, "Valoración del trabajador/a social a través de entrevista personal" },
                    { 116, 32, "1. Estudios sin homologar", true, 2, "Valoración del trabajador/a social a través de entrevista personal" },
                    { 117, 32, "2. Estudios en trámite de homologación", true, 1, "Justificante de solicitud de homologación título" },
                    { 118, 32, "3. Estudios homologados", true, 0, "Certificado de homologación" },
                    { 119, 33, "1. Persona con mucha dificultad para la empleabilidad", true, 3, "Informe o historial emitido por el médico de atención primaria, Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe previo), Informe vida laboral, informe de período de inscripción como solicitante de empleo, del portal de empleo o de las oficinas de empleo" },
                    { 120, 33, "2. Persona con alguna dificultad para la empleabilidad", true, 2, "" },
                    { 121, 33, "3. Persona sin dificultades para la empleabilidad", true, 0, "" },
                    { 122, 34, "1. En paro por menos de un año, con trabajo anterior", true, 1, "Inscripción como demandante de empleo, informe de situación de la oficina de empleo, valoración profesional" },
                    { 123, 34, "2. En paro por menos de un año, sin trabajo anterior", true, 2, "" },
                    { 124, 34, "3. En paro de larga duración (por más de un año) con trabajo anterior", true, 2, "Informe vida laboral expedido por la Seguridad Social" },
                    { 125, 34, "4. En paro de larga duración (por más de un año) sin trabajo anterior", true, 3, "Informe vida laboral expedido por la Seguridad Social" },
                    { 126, 34, "5. Personas inactivas", true, 0, "" },
                    { 127, 35, "1. Trabajo en economía sumergida, trabajo irregular", true, 3, "Informe trabajador/a social de Servicios Sociales o de organización social autorizada (se requiere visita a domicilio salvo que exista informe, Informe vida laboral, informe de movimientos de cuenta bancaria. " },
                    { 128, 35, "2. Trabajo temporal por cuenta ajena, jornada parcial", true, 2, "Copia de contrato, nóminas, Informe vida laboral expedido por la Seguridad Social, copia de contrato" },
                    { 129, 35, "3. Trabajo temporal por cuenta ajena, jornada completa", true, 1, "Copia de contrato, nóminas, Informe vida laboral expedido por la Seguridad Social, copia de contrato" },
                    { 130, 35, "4. Trabajo fijo por cuenta ajena, jornada parcial", true, 1, "Copia de contrato, nóminas, Informe vida laboral expedido por la Seguridad Social, copia de contrato" },
                    { 131, 35, "5. Trabajo fijo por cuenta ajena, jornada completa", true, 0, "Copia de contrato, nóminas, Informe vida laboral expedido por la Seguridad Social, copia de contrato" },
                    { 132, 35, "6. Trabajo por cuenta propia, jornada completa", true, 0, "" },
                    { 133, 35, "7. Trabajo por cuenta propia,  jornada parcial", true, 1, "" },
                    { 134, 35, "8. Otras situaciones precarias de empleo: por ejemplo, que le paguen una parte de la jornada en negro, que no le paguen las horas extra, que compagine un trabajo a jornada parcial con otro en economía sumergida, etc.", true, 2, "Informe vida laboral expedido por la Seguridad Social, informe de período de inscripción como solicitante de empleo, del portal de empleo o de la oficina de empleo" },
                    { 135, 36, "1. Menor o igual de 25 años ", true, 1, "Documento de identidad, pasaporte, cédula de inscripción" },
                    { 136, 36, "2. Entre 26 y 44 años", true, 0, "Documento de identidad, pasaporte, cédula de inscripción" },
                    { 137, 36, "3. Mayor o igual de 45 años", true, 1, "Documento de identidad, pasaporte, cédula de inscripción" },
                    { 138, 37, "1. Está o ha estado bajo tutela", true, 1, "" },
                    { 139, 37, "2. No ha estado bajo tutela", true, 0, "" },
                    { 140, 38, "1. Situación o existencia de indicios de violencia de género", true, 5, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 141, 38, "2. Antecedentes de violencia de género", true, 1, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 142, 39, "3. Desprotección de menores: riesgo grave y desamparo", true, 5, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 143, 39, "4. Desprotección de menores: riesgo moderado o leve", true, 2, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 144, 40, "5. Situación o indicios de maltrato de hijos/as a padres/madres", true, 5, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 145, 40, "6. Abuso económico a mayores", true, 2, "Orden de alejamiento. Sentencia judicial. Denuncia y/o parte de lesiones. Informe de los puntos municipales de violencia de género. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 146, 41, "1. Tiene responsabilidades familiares SIN apoyo: 1 o 2 hijos/as", true, 3, "Libro de familia, partidas de nacimiento, documentación de separación y/o divorcio. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 147, 41, "2. Responsabilidades familiares SIN apoyo: 3 o más hijos/as", true, 4, "Libro de familia, partidas de nacimiento, documentación de separación y/o divorcio. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 148, 41, "3. Responsabilidades familiares CON apoyo: 1 o 2 hijos/as", true, 0, "Libro de familia" },
                    { 149, 41, "4. Responsabilidades familiares CON apoyo: 3 o más hijos/as", true, 1, "" },
                    { 150, 41, "5. Estar a cargo de mayores y/o dependientes", true, 3, "Libro de familia, partidas de nacimiento, documentación de separación y/o divorcio. Informe de organizaciones colaboradoras especializadas. Valoración del trabajador/a social a través de entrevista personal" },
                    { 151, 42, "1. Hacinamiento crítico: menos de 6 m2 por persona", true, 3, "Empadronamiento, informe de habitabilidad, informe social" },
                    { 152, 42, "2. Hacinamiento medio: entre 6 y 10 m2 por persona ", true, 2, "Empadronamiento, informe de habitabilidad, informe social" },
                    { 153, 42, "3. Sin hacinamiento: más de 10 m2 por persona", true, 0, "Empadronamiento, informe de habitabilidad, informe social" },
                    { 154, 43, "1. Dificultades de convivencia y/o relación familiar", true, 2, "Intervenciones del CAI, UME,GRUME, IMMF" },
                    { 155, 43, "2. Vive en un hogar con menores que presentan dificultades de conducta", true, 1, "Denuncia, intervenciones policiales, intervenciones de servicios sociales de atención primaria y/o especializados" },
                    { 156, 44, "1. Tiene suficiente apoyo social y familiar", true, 0, "Valoración del/ de la trabajador/a social" },
                    { 157, 44, "2. Menor de 70 años que no recibe, o recibe insuficiente apoyo social y/o familiar de contenido material, instrumental, cognitivo, emocional", true, 1, "Valoración del/ de la trabajador/a social" },
                    { 158, 44, "3. Mayor de 70 años que  no recibe, o recibe insuficiente apoyo social y/o familiar de contenido material, instrumental, cognitivo, emocional", true, 3, "Valoración del/ de la trabajador/a social" },
                    { 159, 44, "4. Aislamiento, problemas con el entorno", true, 2, "Valoración del/ de la trabajador/a social, visita domiciliaria" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadorFichas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Indicadores",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Dimensiones",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Areas",
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
