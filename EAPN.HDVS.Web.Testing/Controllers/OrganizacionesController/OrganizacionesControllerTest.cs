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

namespace EAPN.HDVS.Web.Testing.Controllers.OrganizacionesController
{
    public class OrganizacionesControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/organizaciones";

        public OrganizacionesControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task ShoudNotAllowedUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario4@test.com");
            var organizacion = new OrganizacionDto { Activa = true, Nombre = "test", Observaciones = "" };

            // Act
            var getResponse = await client.GetAsync(ENDPOINT);
            var getOneResponse = await client.GetAsync($"{ENDPOINT}/1");
            var getMasterdataResponse = await client.GetAsync($"{ENDPOINT}/masterdata");
            var putResponse = await client.PostAsync(ENDPOINT, ClientUtilities.GetRequestContent(organizacion));
            var postResponse = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(organizacion));
            var deleteResponse = await client.DeleteAsync($"{ENDPOINT}/1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, getOneResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getMasterdataResponse.StatusCode);
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldGetOrganizaciones()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<OrganizacionDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldGetMasterdata()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/masterdata");
            var body = await ClientUtilities.GetResponseContent<List<MasterDataDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldGetOneOrganizacion()
        {
            // Arrange
            var id = 1;
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<OrganizacionDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.Equal(id, body.Id);
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldPostOneOrganizacion()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");
            var organizacion = new OrganizacionDto { Activa = true, Nombre = guid, Observaciones = "" };

            // Act
            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(organizacion));
            var body = await ClientUtilities.GetResponseContent<OrganizacionDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Id > 0);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{body.Id}");
            var saved = await ClientUtilities.GetResponseContent<OrganizacionDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(body.Id, saved.Id);
            Assert.Equal(guid, saved.Nombre);
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldPutOneOrganizacion()
        {
            // Arrange
            var id = 3;
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");
            var organizacion = new OrganizacionDto { Activa = true, Nombre = guid, Observaciones = "", Id = id };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(organizacion));
            var body = await ClientUtilities.GetResponseContent<OrganizacionDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<OrganizacionDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(id, saved.Id);
            Assert.Equal(guid, saved.Nombre);
        }

        [Fact]
        [Trait("Category", "OrganizacionesController")]
        public async Task AllowedUserShouldDeleteOneOrganizacion()
        {
            // Arrange
            var id = 3;
            var client = _factory.GetAuthenticatedClientAsync("usuario1@test.com");

            // Act
            var response = await client.DeleteAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<OrganizacionDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<OrganizacionDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, savedResponse.StatusCode);
            Assert.Null(saved);
        }
    }
}
