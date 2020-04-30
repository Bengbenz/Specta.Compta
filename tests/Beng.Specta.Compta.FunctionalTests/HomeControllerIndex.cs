using System;
using System.Net.Http;
using System.Threading.Tasks;
using Beng.Specta.Compta.Server;
using Xunit;

namespace Beng.Specta.Compta.FunctionalTests
{
    public class HomeControllerIndex : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public HomeControllerIndex(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task ReturnsViewWithCorrectMessage()
        {
            var response = await _client.GetAsync(new Uri("/", UriKind.Relative)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.Contains("FinProd", stringResponse, StringComparison.InvariantCulture);
        }
    }
}
