using EAPN.HDVS.Entities;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Testing.Controllers.PerfilesController;
using EAPN.HDVS.Web.Testing.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.UsuariosController
{
    public class UsuariosControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/usuarios";

        public UsuariosControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
            _factory.AddAdditionalSeeder(new UsuariosControllerDBSeeder());
        }

        private UsuarioDto CreateUsuarioDto(int id, int organizacionId, int perfilId, string observaciones)
        {
            return new UsuarioDto
            {
                Activo = true,
                Apellidos = $"Apellidos",
                OrganizacionId = organizacionId,
                Email = $"usuario{id}@test.com",
                Id = id,
                Clave = "test",
                Nombre = $"Nombre",
                Observaciones = observaciones,
                Perfiles = new List<MasterDataDto>(new[] { new MasterDataDto { Id = perfilId } })
            };
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task AllowedSuperadminUserShouldGetUsuarios()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario11@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<UsuarioDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());
            Assert.Contains(body, x => x.OrganizacionId != 1);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task AllowedUserShouldGetOneUsuario()
        {
            // Arrange
            var id = 4;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<UsuarioDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.Equal(id, body.Id);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task AllowedUserShouldPostOneUsuario()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");
            var usuario = CreateUsuarioDto(0, 1, 1, guid);

            // Act
            var response = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(usuario));
            var body = await ClientUtilities.GetResponseContent<UsuarioDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Id > 0);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{body.Id}");
            var saved = await ClientUtilities.GetResponseContent<UsuarioDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(body.Id, saved.Id);
            Assert.Equal(guid, saved.Observaciones);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task AllowedUserShouldPutOneUsuario()
        {
            // Arrange
            var id = 25;
            var guid = Guid.NewGuid().ToString();
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");
            var usuario = CreateUsuarioDto(id, 1, 2, guid);

            // Act
            var response = await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(usuario));
            var body = await ClientUtilities.GetResponseContent<UsuarioDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<UsuarioDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.OK, savedResponse.StatusCode);
            Assert.NotNull(saved);
            Assert.Equal(id, saved.Id);
            Assert.Equal(guid, saved.Observaciones);
        }

        [Fact]
        [Trait("Category", "UsuariosController")]
        public async Task AllowedUserShouldDeleteOneUsuario()
        {
            // Arrange
            var id = 30;
            var client = _factory.GetAuthenticatedClientAsync("usuario10@test.com");

            // Act
            var response = await client.DeleteAsync($"{ENDPOINT}/{id}");
            var body = await ClientUtilities.GetResponseContent<UsuarioDto>(response);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(body);

            // Act
            var savedResponse = await client.GetAsync($"{ENDPOINT}/{id}");
            var saved = await ClientUtilities.GetResponseContent<UsuarioDto>(savedResponse);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, savedResponse.StatusCode);
            Assert.Null(saved);
        }
    }
}
