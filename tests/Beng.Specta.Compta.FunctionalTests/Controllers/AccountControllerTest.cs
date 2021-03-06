﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

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
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            _serviceProvider = factory?.Services;
            _userManager = _serviceProvider.GetService<UserManager<IdentityUser>>();
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task Details_ReturnsLoggedOutUser()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("api/account/details", UriKind.Relative));

            //VERIFY
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task Details_ReturnNotFound()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("api/account/details/Super@g1.com", UriKind.Relative));

            //VERIFY
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task Details_GetUserInfoByEmail()
        {
            //SETUP
            var registerModel = new RegisterUserInfoDTO
            {
                UserName = "Kevin01",
                Email = "Kevin@g1.",
                NewPassword = "Kevin@g1."
            };
            await RegisterUserAsync(registerModel);

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("api/account/details/Kevin@g1.com", UriKind.Relative));

            //VERIFY
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseAsString);
            Assert.NotEmpty(result);
            Assert.True(result.ContainsKey("email"));
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task SignIn_ReturnsLoggedUser()
        {
            //SETUP
            var registerModel = new RegisterUserInfoDTO
            {
                UserName = "SuperAdmin",
                Email = "Super@g1.com",
                NewPassword = "Super@g1.com"
            };
            await RegisterUserAsync(registerModel);

            var loginModel = new SignInUserInfoDTO
            {
                Email = registerModel.Email,
                Password = registerModel.NewPassword
            };
            
            //ATTEMPT
            var response = await _client.PostAsJsonAsync("api/account/signin", loginModel);

            //VERIFY
            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }

        [Fact(Skip="To fix later, fix UserManager service")]
        public async Task Register_ReturnRegistredUser()
        {
            //SETUP
            var model = new RegisterUserInfoDTO
            {
                UserName = "Leo46",
                Email = "Leo46@g1.com",
                NewPassword = "Leo46@g1.com"
            };

            //ATTEMPT
            var response = await _client.PostAsJsonAsync("api/account/register", model);
            
            //VERIFY
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserInfoDTO>(responseAsString);

            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Equal(model.UserName, result.UserName);
            Assert.Equal(model.Email, result.Email);
        }

        private async Task RegisterUserAsync(RegisterUserInfoDTO model)
        {
            var authRepository = GetAuthorizationRepository();

            var identity = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(identity, model.NewPassword);

            Assert.True(result.Succeeded);
        }
    }
}
