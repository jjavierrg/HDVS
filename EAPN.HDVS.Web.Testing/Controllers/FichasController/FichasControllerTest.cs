using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Testing.Utils;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.FichasController
{
    public class FichasControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/fichas";

        public FichasControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
            _factory.AddAdditionalSeeder(new FichasControllerDBSeeder());
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
        public async Task ShoudUpdateFotoDataOnAdd()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClientAsync("usuario12@test.com");
            var ficha = GetNewFicha();
            var fotoId = 9;

            // Act
            ficha.FotoId = fotoId;
            ficha.UsuarioId = 12;
            ficha.OrganizacionId = 1;

            var responseFicha = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(ficha));
            var fichaCreated = await ClientUtilities.GetResponseContent<FichaDto>(responseFicha);

            var response = await client.GetAsync($"/api/adjuntos/{fotoId}");
            var foto = await ClientUtilities.GetResponseContent<AdjuntoDto>(response);

            // Assert
            Assert.NotNull(foto);
            Assert.NotNull(fichaCreated);

            Assert.Equal(foto.Id, fichaCreated.FotoId);
            Assert.Equal(foto.FichaId, fichaCreated.Id);
            Assert.Equal(foto.OrganizacionId, fichaCreated.OrganizacionId);
        }
    }
}
