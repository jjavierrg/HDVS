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

namespace EAPN.HDVS.Web.Testing.Controllers.SituacionesAdministrativasController
{
    public class SituacionesAdministrativasControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/situacionesAdministrativas";

        public SituacionesAdministrativasControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
            _factory.AddAdditionalSeeder(new SituacionesAdministrativasDBSeeder());
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task ShouldNotAllowedUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");
            var situacionAdministrativa = new SituacionAdministrativaDto { Id = 1, Descripcion = "Test" };

            // Act
            var getResponse = await client.GetAsync(ENDPOINT);
            var getOneResponse = await client.GetAsync($"{ENDPOINT}/1");
            var putResponse = await client.PostAsync(ENDPOINT, ClientUtilities.GetRequestContent(situacionAdministrativa));
            var postResponse = await client.PutAsync($"{ENDPOINT}/1", ClientUtilities.GetRequestContent(situacionAdministrativa));
            var deleteResponse = await client.DeleteAsync($"{ENDPOINT}/1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, getOneResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task ShouldAllowedUserToMasterdata()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/masterdata");
            var body = await ClientUtilities.GetResponseContent<List<SituacionAdministrativaDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldGetSituacionesAdministrativas()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<SituacionAdministrativaDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldGetMasterdata()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/masterdata");
            var body = await ClientUtilities.GetResponseContent<List<MasterDataDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldGetOneSituacionAdministrativa()
        {
            // Arrange
            var id = 1;
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.Equal(id, body.Id);
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldPostOneSituacionAdministrativa()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");
            var situacionAdministrativa = new SituacionAdministrativaDto { Descripcion = guid };

            // Act
            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(situacionAdministrativa));
            var body = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Id > 0);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{body.Id}");
            var saved = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(body.Id, saved.Id);
            Assert.Equal(guid, saved.Descripcion);
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldPutOneSituacionAdministrativa()
        {
            // Arrange
            var id = 10;
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");
            var situacionAdministrativa = new SituacionAdministrativaDto { Descripcion = guid, Id = id };

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(situacionAdministrativa));
            var body = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(id, saved.Id);
            Assert.Equal(guid, saved.Descripcion);
        }

        [Fact]
        [Trait("Category", "SituacionesAdministrativasController")]
        public async Task AllowedUserShouldDeleteOneSituacionAdministrativa()
        {
            // Arrange
            var id = 20;
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.DeleteAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<SituacionAdministrativaDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, savedResponse.StatusCode);
            Assert.Null(saved);
        }
    }
}
