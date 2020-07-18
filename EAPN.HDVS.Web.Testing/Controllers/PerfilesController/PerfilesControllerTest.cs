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

namespace EAPN.HDVS.Web.Testing.Controllers.PerfilesController
{
    public class PerfilesControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/perfiles";

        public PerfilesControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
            _factory.AddAdditionalSeeder(new PerfilesControllerDBSeeder());
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task ShouldNotAllowedUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");
            var perfil = new PerfilDto { Descripcion = "test", Permisos = new List<MasterDataDto>() };

            // Act
            var getResponse = await client.GetAsync(ENDPOINT);
            var getOneResponse = await client.GetAsync($"{ENDPOINT}/1");
            var getMasterdataResponse = await client.GetAsync($"{ENDPOINT}/masterdata");
            var putResponse = await client.PostAsync(ENDPOINT, ClientUtilities.GetRequestContent(perfil));
            var postResponse = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(perfil));
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
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldGetPerfiles()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<PerfilDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldGetMasterdata()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/masterdata");
            var body = await ClientUtilities.GetResponseContent<List<MasterDataDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task NotAdminShouldGetNotAdminProfiles()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/masterdata");
            var body = await ClientUtilities.GetResponseContent<List<MasterDataDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.DoesNotContain(body, x => x.Id == 4);
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldGetOnePerfil()
        {
            // Arrange
            var id = 1;
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<PerfilDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.Equal(id, body.Id);
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldPostOnePerfil()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");
            var perfil = new PerfilDto { Descripcion = guid, Permisos = new List<MasterDataDto>() };

            // Act
            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(perfil));
            var body = await ClientUtilities.GetResponseContent<PerfilDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Id > 0);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{body.Id}");
            var saved = await ClientUtilities.GetResponseContent<PerfilDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(body.Id, saved.Id);
            Assert.Equal(guid, saved.Descripcion);
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldPutOnePerfil()
        {
            // Arrange
            var id = 2;
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");
            var perfil = new PerfilDto { Id = id, Descripcion = guid };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(perfil));
            var body = await ClientUtilities.GetResponseContent<PerfilDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<PerfilDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(id, saved.Id);
            Assert.Equal(guid, saved.Descripcion);
        }

        [Fact]
        [Trait("Category", "PerfilesController")]
        public async Task AllowedUserShouldDeleteOnePerfil()
        {
            // Arrange
            var id = 3;
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.DeleteAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<PerfilDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<PerfilDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, savedResponse.StatusCode);
            Assert.Null(saved);
        }
    }
}
