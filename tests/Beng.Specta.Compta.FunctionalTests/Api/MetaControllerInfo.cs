using System;
using System.Net.Http;
using System.Threading.Tasks;
using Beng.Specta.Compta.Server;
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
            var response = await _client.GetAsync(new Uri("/info", UriKind.Relative)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.Contains("Version", stringResponse, StringComparison.InvariantCulture);
            Assert.Contains("Last Updated", stringResponse, StringComparison.InvariantCulture);
        }
    }
}
