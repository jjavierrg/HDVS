using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Testing.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.ConfiguracionesController
{
    public class ConfiguracionesControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/configuraciones";

        public ConfiguracionesControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Category", "ConfiguracionesController")]
        public async Task ShoudAllUserGetConfiguracion()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/1");
            var body = await ClientUtilities.GetResponseContent<ConfiguracionDto>(response);

            var responseAll = await client.GetAsync($"{ENDPOINT}");
            var bodyAll = await ClientUtilities.GetResponseContent<List<ConfiguracionDto>>(responseAll);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAll.StatusCode);

            Assert.NotNull(body);
            Assert.NotNull(bodyAll);
            Assert.True(bodyAll.Any());
        }

        [Fact]
        [Trait("Category", "ConfiguracionesController")]
        public async Task ShoudNotAdminNoUpdateConfiguracion()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");
            var dto = new ConfiguracionDto { Id = 1, Enlaces = "updated", MostrarEnlaces = false };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(dto));
            var body = await ClientUtilities.GetResponseContent<ConfiguracionDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Null(body);
        }

        [Fact]
        [Trait("Category", "ConfiguracionesController")]
        public async Task ShoudAdminUpdateConfiguracion()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");
            var guid = Guid.NewGuid().ToString();
            var dto = new ConfiguracionDto { Id = 1, Enlaces = guid, MostrarEnlaces = false };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(dto));
            var readRequest = await client.GetAsync($"{ENDPOINT}/1");
            var body = await ClientUtilities.GetResponseContent<ConfiguracionDto>(readRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, readRequest.StatusCode);
            Assert.NotNull(body);
            Assert.Equal(guid, body.Enlaces);
        }
    }
}
