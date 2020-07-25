using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Testing.Utils;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.DimensionesController
{
    public class DimensionesControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/dimensiones";

        public DimensionesControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Category", "DimensionesController")]
        public async Task ShouldNotAllowedUser()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario4@test.com");

            // Act
            var getResponse = await client.GetAsync(ENDPOINT);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, getResponse.StatusCode);
        }

        [Fact]
        [Trait("Category", "DimensionesController")]
        public async Task AllowedUserShouldGetDimensiones()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var response = await client.GetAsync(ENDPOINT);
            var body = await ClientUtilities.GetResponseContent<List<DimensionDto>>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.True(body.Any());

            // Not inactive data and order
            body.Should().BeInAscendingOrder(x => x.Orden);
            Assert.DoesNotContain(body, x => x.Orden == 5 || x.Orden == 10);

            var categorias = body.SelectMany(x => x.Categorias).ToList();
            categorias.ForEach(x => x.DimensionId = x.DimensionId * 100 + x.Orden);
            categorias.Should().BeInAscendingOrder(x => x.DimensionId);
            Assert.DoesNotContain(categorias, x => x.Orden == 3 || x.Orden == 6 || x.Orden == 9);

            var indicadores = categorias.SelectMany(x => x.Indicadores).ToList();
            indicadores.ForEach(x => x.CategoriaId = x.CategoriaId * 1000 + x.Orden);
            indicadores.Should().BeInAscendingOrder(x => x.CategoriaId);
            Assert.DoesNotContain(indicadores, x => x.Orden == 4 || x.Orden == 8);
        }
    }
}
