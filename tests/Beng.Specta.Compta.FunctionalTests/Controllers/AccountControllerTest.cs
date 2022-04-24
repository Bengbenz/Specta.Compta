using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.DTOs.Auth;
using Beng.Specta.Compta.IntegrationTests.Data;
using Beng.Specta.Compta.Server;

using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Controllers
{
    public class AccountControllerTest : BaseRepositoryTestFixture, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AccountControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClientWithoutAuthorization();
        }

        [Fact]
        public async Task Details_ReturnsLoggedOutUser()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/account/details", UriKind.Relative));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }

        [Fact]
        public async Task Details_ReturnNotFound()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/account/details/Super@g1.com", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task Details_GetUserInfoByEmail()
        {
            // Arrange
            var registerModel = new RegisterUserInfoDTO
            {
                UserName = "Kevin01",
                Email = "Kevin@g1.",
                NewPassword = "Kevin@g1."
            };
            //await RegisterUserAsync(registerModel);

            // Act
            var response = await _client.GetAsync(new Uri("api/account/details/Kevin@g1.com", UriKind.Relative));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseAsString);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.ContainsKey("email"));
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task SignIn_ReturnsLoggedUser()
        {
            // Arrange
            var registerModel = new RegisterUserInfoDTO
            {
                UserName = "SuperAdmin",
                Email = "Super@g1.com",
                NewPassword = "Super@g1.com"
            };
            //await RegisterUserAsync(registerModel);

            var loginModel = new SignInUserInfoDTO
            {
                Email = registerModel.Email,
                Password = registerModel.NewPassword
            };
            
            // Act
            var response = await _client.PostAsJsonAsync("api/account/signin", loginModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }

        [Fact]
        public async Task Register_ReturnRegisterUser()
        {
            // Arrange
            var model = new RegisterUserInfoDTO
            {
                UserName = "Leo46",
                Email = "Leo46@g1.com",
                NewPassword = "Leo46@g1.com",
                ConfirmPassword = "Leo46@g1.com",
                AgreeWithTerms = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/account/register", model);
            
            // Assert
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Equal(model.UserName, result.UserName);
            Assert.Equal(model.Email, result.Email);
        }

        // private async Task RegisterUserAsync(RegisterUserInfoDTO model)
        // {
        //     var identity = new IdentityUser { UserName = model.UserName, Email = model.Email };
        //     var result = await _userManager.CreateAsync(identity, model.NewPassword);
        //
        //     Assert.True(result.Succeeded);
        // }
    }
}
