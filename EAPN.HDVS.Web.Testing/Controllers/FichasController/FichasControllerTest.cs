using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Testing.Utils;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.FichasController
{
    public class FichasControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;
        private const string ENDPOINT = "/api/fichas";
        private const string ENDPOINT_SEGUIMIENTOS = "/api/seguimientos";

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
                OrganizacionId = 1,
                UsuarioId = 12,
            };
        }

        [Fact]
        [Trait("Category", "FichasController")]
        public async Task ShouldUpdateFotoDataOnAdd()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario12@test.com");
            var ficha = GetNewFicha();
            var fotoId = 9;

            // Act
            ficha.FotoId = fotoId;

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

        [Fact]
        [Trait("Category", "FichasController")]
        public async Task ShouldGetDatosFicha()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario12@test.com");
            var id = 15;

            // Act
            var response = await client.GetAsync($"{ENDPOINT}/{id}/datos");
            var ficha = await ClientUtilities.GetResponseContent<DatosFichaDto>(response);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(ficha);

            Assert.Equal(id, ficha.Id);
        }

        [Fact]
        [Trait("Category", "FichasController")]
        public async Task ShouldMarkAsComplete()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario12@test.com");
            var ficha = GetNewFicha();
            var seguimiento = new SeguimientoDto { Completo = true, OrganizacionId = 1, UsuarioId = 12 };

            // Act
            ficha.DatosCompletos = true;
            var responseFicha = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(ficha));
            var id = (await ClientUtilities.GetResponseContent<DatosFichaDto>(responseFicha)).Id;
            ficha.Id = id;

            seguimiento.FichaId = id;
            var responseSeguimiento = await client.PostAsync($"{ENDPOINT_SEGUIMIENTOS}", ClientUtilities.GetRequestContent(seguimiento));
            var idSeguimiento = (await ClientUtilities.GetResponseContent<SeguimientoDto>(responseSeguimiento)).Id;
            seguimiento.Id = idSeguimiento;

            // Assert
            await VerifyFichaCompleta(id, true);

            // Set ficha with datosIncompletos as false and then back to true
            ficha.DatosCompletos = false;
            await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(ficha));
            await VerifyFichaCompleta(id, false);

            ficha.DatosCompletos = true;
            await client.PutAsync($"{ENDPOINT}/{id}", ClientUtilities.GetRequestContent(ficha));
            await VerifyFichaCompleta(id, true);

            // Set seguimiento with completo as false and then back to true
            seguimiento.Completo = false;
            await client.PutAsync($"{ENDPOINT_SEGUIMIENTOS}/{idSeguimiento}", ClientUtilities.GetRequestContent(seguimiento));
            await VerifyFichaCompleta(id, false);

            seguimiento.Completo = true;
            await client.PutAsync($"{ENDPOINT_SEGUIMIENTOS}/{idSeguimiento}", ClientUtilities.GetRequestContent(seguimiento));
            await VerifyFichaCompleta(id, true);
        }

        [Fact]
        [Trait("Category", "FichasController")]
        public async Task ShouldOnlyUpdateOnLastSeguimiento()
        {
            // Arrange
            var client = _factory.GetAuthenticatedClient("usuario12@test.com");
            var ficha = GetNewFicha();
            var seguimiento = new SeguimientoDto { Completo = true, OrganizacionId = 1, UsuarioId = 12, Fecha = DateTime.Now };
            var futureSeguimiento = new SeguimientoDto { Completo = false, OrganizacionId = 1, UsuarioId = 12, Fecha = DateTime.Now.AddDays(1) };
            var pastSeguimiento = new SeguimientoDto { Completo = false, OrganizacionId = 1, UsuarioId = 12, Fecha = DateTime.Now.AddDays(-1) };

            // Act
            ficha.DatosCompletos = true;
            var responseFicha = await client.PostAsync($"{ENDPOINT}", ClientUtilities.GetRequestContent(ficha));
            var id = (await ClientUtilities.GetResponseContent<DatosFichaDto>(responseFicha)).Id;

            ficha.Id = id;
            seguimiento.FichaId = id;
            futureSeguimiento.FichaId = id;
            pastSeguimiento.FichaId = id;

            var responseSeguimiento = await client.PostAsync($"{ENDPOINT_SEGUIMIENTOS}", ClientUtilities.GetRequestContent(seguimiento));
            var idSeguimiento = (await ClientUtilities.GetResponseContent<SeguimientoDto>(responseSeguimiento)).Id;
            seguimiento.Id = idSeguimiento;

            Assert.True(responseSeguimiento.IsSuccessStatusCode);
            await VerifyFichaCompleta(id, true);

            // Add past seguimiento
            responseSeguimiento = await client.PostAsync($"{ENDPOINT_SEGUIMIENTOS}", ClientUtilities.GetRequestContent(pastSeguimiento));
            Assert.True(responseSeguimiento.IsSuccessStatusCode);
            await VerifyFichaCompleta(id, true);

            // Add future seguimiento
            responseSeguimiento = await client.PostAsync($"{ENDPOINT_SEGUIMIENTOS}", ClientUtilities.GetRequestContent(futureSeguimiento));
            Assert.True(responseSeguimiento.IsSuccessStatusCode);
            await VerifyFichaCompleta(id, false);

            // Update past seguimiento and verify no flag update
            responseSeguimiento = await client.PutAsync($"{ENDPOINT_SEGUIMIENTOS}/{idSeguimiento}", ClientUtilities.GetRequestContent(seguimiento));
            Assert.True(responseSeguimiento.IsSuccessStatusCode);
            await VerifyFichaCompleta(id, false);

            // Update seguimiento date and verify flag update
            seguimiento.Fecha = DateTime.Now.AddDays(2);
            responseSeguimiento = await client.PutAsync($"{ENDPOINT_SEGUIMIENTOS}/{idSeguimiento}", ClientUtilities.GetRequestContent(seguimiento));
            Assert.True(responseSeguimiento.IsSuccessStatusCode);
            await VerifyFichaCompleta(id, true);
        }

        private async Task VerifyFichaCompleta(int fichaId, bool expectedValue)
        {
            var client = _factory.GetAuthenticatedClient("usuario12@test.com");

            var response = await client.GetAsync($"{ENDPOINT}/{fichaId}");
            var fichaRequest = await ClientUtilities.GetResponseContent<FichaDto>(response);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(fichaId, fichaRequest.Id);
            Assert.NotNull(fichaRequest);
            Assert.Equal(expectedValue, fichaRequest.Completa);
        }
    }
}
