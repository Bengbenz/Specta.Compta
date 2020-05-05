using System;
using System.Net.Http;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Server;

using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Api
{
    public class UserController : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UserController(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task ReturnsUserInfo()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("/user", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<UserInfoDTO>(stringResponse);

            //VERIFY
            // LoggedOutUser
            Assert.NotNull(result);
            Assert.False(result.IsAuthenticated);
            Assert.Empty(result.UserName);
        }
    }
}
