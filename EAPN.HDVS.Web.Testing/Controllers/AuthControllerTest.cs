﻿using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Testing.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace EAPN.HDVS.Web.Testing.Controllers
{
    public class AuthControllerTest : IClassFixture<ApiFactory<Startup>>
    {
        private readonly ApiFactory<Startup> _factory;

        public AuthControllerTest(ApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
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
        public async Task ShouldActiveUserRefreshToken()
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
            Assert.NotEmpty(token.RefreshToken);

            // Act
            var refresh = new RefreshTokenAttempDto { UserId = 1, RefreshToken = token.RefreshToken };
            var refreshResponse = await client.PostAsync("/api/auth/refresh/", ClientUtilities.GetRequestContent(refresh));

            // Assert
            refreshResponse.EnsureSuccessStatusCode();
            var refreshed = await ClientUtilities.GetResponseContent<UserTokenDto>(refreshResponse);
            Assert.NotNull(refreshed);
            Assert.NotEmpty(refreshed.AccessToken);
            Assert.NotEmpty(refreshed.RefreshToken);
            Assert.NotEqual(token.RefreshToken, refreshed.RefreshToken);
        }

        [Fact]
        public async Task ShouldNotRefreshOldToken()
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
            Assert.NotEmpty(token.RefreshToken);

            // Act
            var refresh = new RefreshTokenAttempDto { UserId = 1, RefreshToken = token.RefreshToken };
            var refreshResponse = await client.PostAsync("/api/auth/refresh/", ClientUtilities.GetRequestContent(refresh));

            // Assert
            refreshResponse.EnsureSuccessStatusCode();
            var refreshed = await ClientUtilities.GetResponseContent<UserTokenDto>(refreshResponse);
            Assert.NotNull(refreshed);
            Assert.NotEmpty(refreshed.AccessToken);
            Assert.NotEmpty(refreshed.RefreshToken);
            Assert.NotEqual(token.RefreshToken, refreshed.RefreshToken);

            // Act
            var refreshOldToken = new RefreshTokenAttempDto { UserId = 1, RefreshToken = token.RefreshToken };
            var refreshOldResponse = await client.PostAsync("/api/auth/refresh/", ClientUtilities.GetRequestContent(refreshOldToken));

            // Assert
            refreshOldResponse.EnsureSuccessStatusCode();
            var refreshedOld = await ClientUtilities.GetResponseContent<UserTokenDto>(refreshOldResponse);
            Assert.Null(refreshedOld);
        }

        [Fact]
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
