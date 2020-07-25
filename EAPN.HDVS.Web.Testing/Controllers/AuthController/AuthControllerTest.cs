using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Testing.Utils;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers.AuthController
{
    public class AuthControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;

        public AuthControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Category", "AuthController")]
        public async Task ShouldActiveUserLogin()
        {
            // Arrange
            var client = _factory.GetAnonymousClient();

            // Act
            var attemp = new LoginAttempDto { Email = "usuario1@test.com", Password = "pass" };
            var response = await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(attemp));

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await ClientUtilities.GetResponseContent<UserTokenDto>(response);
            Assert.NotNull(token);
            Assert.NotEmpty(token.AccessToken);
        }

        [Fact]
        [Trait("Category", "AuthController")]
        public async Task ShouldActiveUserRefreshToken()
        {
            // Arrange
            var client = _factory.GetAnonymousClient();
            var autorizedClient = _factory.GetAuthenticatedClient("usuario1@test.com");

            // Act
            var attemp = new LoginAttempDto { Email = "usuario1@test.com", Password = "pass" };
            var response = await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(attemp));

            // Assert
            response.EnsureSuccessStatusCode();
            var token = await ClientUtilities.GetResponseContent<UserTokenDto>(response);
            Assert.NotNull(token);
            Assert.NotEmpty(token.AccessToken);

            // Act
            var refreshResponse = await autorizedClient.GetAsync("/api/auth/refresh/");

            // Assert
            refreshResponse.EnsureSuccessStatusCode();
            var refreshed = await ClientUtilities.GetResponseContent<UserTokenDto>(refreshResponse);
            Assert.NotNull(refreshed);
            Assert.NotEmpty(refreshed.AccessToken);
        }

        [Fact]
        [Trait("Category", "AuthController")]
        public async Task ShouldInactiveUserNotLogin()
        {
            // Arrange
            var client = _factory.GetAnonymousClient();

            // Act
            var attemp = new LoginAttempDto { Email = "usuario7@test.com", Password = "pass" };
            var response = await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(attemp));

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await ClientUtilities.GetResponseContent<UserTokenDto>(response);
            Assert.Null(token);
        }

        [Fact]
        [Trait("Category", "AuthController")]
        public async Task ShouldActiveUserInactivePartnerNotLogin()
        {
            // Arrange
            var client = _factory.GetAnonymousClient();

            // Act
            var attemp = new LoginAttempDto { Email = "usuario3@test.com", Password = "pass" };
            var response = await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(attemp));

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await ClientUtilities.GetResponseContent<UserTokenDto>(response);
            Assert.Null(token);
        }

        [Fact]
        [Trait("Category", "AuthController")]
        public async Task ShouldActiveUserGetBlocked()
        {
            // Arrange
            var client = _factory.GetAnonymousClient();

            // Act
            for (int i = 1; i <= 6; i++)
            {
                var failAttemp = new LoginAttempDto { Email = "usuario5@test.com", Password = "fail" };
                await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(failAttemp));
            }

            var attemp = new LoginAttempDto { Email = "usuario5@test.com", Password = "pass" };
            var response = await client.PostAsync("/api/auth/", ClientUtilities.GetRequestContent(attemp));

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await ClientUtilities.GetResponseContent<UserTokenDto>(response);
            Assert.Null(token);
        }
    }
}
