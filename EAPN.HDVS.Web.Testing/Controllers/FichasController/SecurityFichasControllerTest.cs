using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Testing.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.FichasController
{
    public class SecurityFichasControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/fichas";

        public SecurityFichasControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        private FichaDto GetNewFicha(int? id = null)
        {
            return new FichaDto
            {
                Id = id ?? 0,
                Nombre = "Nombre",
                Codigo = "Codigo",
                FotocopiaDNI = false,
                PoliticaFirmada = false,
                Apellido1 = "Apellido1",
                Apellido2 = "Apellido2",
                OrganizacionId = 0,
                UsuarioId = 0
            };
        }

        [Fact]
        [Trait("Category", "FichasController")]
        public async Task ShoudNotCreateFichaForOtherAsociacion()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");
            var dto = GetNewFicha();
            var fotoId = 9;

            // Act
            dto.FotoId = fotoId;
            dto.UsuarioId = 12;
            dto.OrganizacionId = 2;

            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(dto));
            var ficha= await ClientUtilities.GetResponseContent<FichaDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, ficha.Id);
            Assert.Contains(_factory.Logger.LogEntries, x => x.LogLevel == LogLevel.Critical && x.Message.Contains("[Fichas] Se ha intentado crear una ficha para otra organizacion"));
        }
    }
}
