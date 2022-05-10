using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.DTOs.Auth;
using Beng.Specta.Compta.IntegrationTests.Data;
using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Controllers
{
    public class AccountControllerTest : BaseRepositoryTestFixture, IClassFixture<IntegrationTestingWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AccountControllerTest(IntegrationTestingWebApplicationFactory factory)
        {
            _client = factory.CreateClientWithFakeAuthorization();
        }

        [Fact]
        public async Task Details_WithoutEmail_ReturnsLoggedOutUser()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/account/details", UriKind.Relative));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDto>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }

        [Fact]
        public async Task Details_WithUnknownEmail_ReturnsNotFound()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/account/details/Super@g1.com", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_WithKnownEmail_ReturnsUserInfo()
        {
            // Arrange
            var registerModel = await RegisterUserAsync("Kevin01", true);

            // Act
            var response = await _client.GetAsync(new Uri($"api/account/details/{registerModel.Email}", UriKind.Relative));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDto>(responseAsString);
            Assert.NotNull(result);
            Assert.Equal(registerModel.UserName, result.UserName);
            Assert.Equal(registerModel.Email, result.Email);
        }

        [Fact]
        public async Task SignIn_WithKnownCredentials_ReturnsLoggedUser()
        {
            // Arrange
            var registerModel =  await RegisterUserAsync("SuperAdmin", true);
            var loginModel = new SignInUserInfoDto
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };
            
            // Act
            var response = await _client.PostAsJsonAsync("api/account/signin", loginModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDto>(responseAsString);

            Assert.NotNull(result);
            Assert.True(result.IsAuthenticated);
            Assert.Equal(registerModel.UserName, result.UserName);
            Assert.Equal(registerModel.Email, result.Email);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsSavedUser()
        {
            // Arrange
            var model = await RegisterUserAsync("Leo45");

            // Act
            var response = await _client.PostAsJsonAsync("api/account/register", model);
            
            // Assert
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDto>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Equal(model.UserName, result.UserName);
            Assert.Equal(model.Email, result.Email);
        }
        
        [Fact]
        public async Task Register_SameUserTwice_ReturnsBadRequest()
        {
            // Arrange
            var model = await RegisterUserAsync("Leo46", true);

            // Act
            var response = await _client.PostAsJsonAsync("api/account/register", model);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            string responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseAsString);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        private async Task<RegisterUserInfoDto> RegisterUserAsync(string userName, bool shouldPersist = false)
        {
            var model = new RegisterUserInfoDto
            {
                UserName = userName,
                Email = $"{userName}@g1.com",
                Password = "Leo46@g1.com",
                ConfirmPassword = "Leo46@g1.com",
                AgreeWithTerms = true
            };
            if (!shouldPersist)
            {
                return model;
            }
            var response = await _client.PostAsJsonAsync("api/account/register", model);
            response.EnsureSuccessStatusCode();
            return model;
        }
    }
}
