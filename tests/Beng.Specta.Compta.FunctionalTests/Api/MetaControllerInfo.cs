using System;
using System.Net.Http;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Server;

using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Api
{
    public class MetaControllerInfo : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public MetaControllerInfo(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task ReturnsVersionAndLastUpdateDate()
        {
            var response = await _client.GetAsync(new Uri("/info", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Version", stringResponse, StringComparison.InvariantCulture);
            Assert.Contains("Last Updated", stringResponse, StringComparison.InvariantCulture);
        }

        [Fact]
        public async Task ReturnsTenantInfo()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("/tenant", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TenantInfoDTO>(stringResponse);

            //VERIFY
            Assert.NotNull(result);
            Assert.NotEqual("Inconnu", result.Identifier);
            Assert.Equal("finprod", result.Id);
            Assert.Equal(result.Id, result.Identifier);
            Assert.NotEmpty(result.Name);
        }
    }
}
