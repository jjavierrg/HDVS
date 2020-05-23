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

namespace EAPN.HDVS.Web.Testing.Controllers.UsuariosController
{
    public class SecurityUsuariosControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/usuarios";

        public SecurityUsuariosControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task ShoudNotAllowedUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario4@test.com");
            var usuario = new UsuarioDto { Nombre = "test" };

            // Act
            var getResponse = await client.GetAsync(ENDPOINT);
            var getOneResponse = await client.GetAsync($"{ENDPOINT}/1");
            var getMasterdataResponse = await client.GetAsync($"{ENDPOINT}/masterdata");
            var putResponse = await client.PostAsync(ENDPOINT, ClientUtilities.GetRequestContent(usuario));
            var postResponse = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(usuario));
            var deleteResponse = await client.DeleteAsync($"{ENDPOINT}/1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, getOneResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, getMasterdataResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task NotAdminShouldNotGetAdminUsers()
        {
            // Arrange
            var adminId = 1;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}");
            var body = await ClientUtilities.GetResponseContent<IEnumerable<UsuarioDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
            Assert.DoesNotContain(body, x => x.Id == adminId);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task NotAdminShouldNotGetAdminUser()
        {
            // Arrange
            var id = 1;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<UsuarioDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotGetOtherPartnerUsers()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<UsuarioDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
            Assert.DoesNotContain(body, x => x.OrganizacionId != 1);
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotGetOtherPartnerUser()
        {
            // Arrange
            var id = 2;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<List<UsuarioDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotCreateOtherPartnerUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");
            var usuario = new UsuarioDto { };

            // Act
            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(usuario));

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Contains(_factory.Logger.LogEntries, x => x.LogLevel == LogLevel.Critical && x.Message.Contains("no autorizada: Se ha intentado crear un usuario de otra organización"));
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotUpdateOtherPartnerUser()
        {
            // Arrange
            var id = 2;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");
            var usuario = new UsuarioDto { Id = id, OrganizacionId = 2 };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(usuario));

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Contains(_factory.Logger.LogEntries, x => x.LogLevel == LogLevel.Critical && x.Message.Contains("Actualización para el usuario"));
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotChangeUserToOtherPartner()
        {
            // Arrange
            var id = 1;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");
            var usuario = new UsuarioDto { Id = id, OrganizacionId = 2 };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(usuario));

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Contains(_factory.Logger.LogEntries, x => x.LogLevel == LogLevel.Critical && x.Message.Contains("Cambio de organización para el usuario"));
        }

        [Fact]
        [Trait("Category", "SecurityUsuariosController")]
        public async Task NotAdminShouldNotDeleteOtherPartneUsers()
        {
            // Arrange
            var id = 2;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.DeleteAsync($"{ENDPOINT}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Contains(_factory.Logger.LogEntries, x => x.LogLevel == LogLevel.Critical && x.Message.Contains("no autorizada: El usuario pertenece a otra organización"));
        }
    }
}
