using System;
using System.Net.Http;
using System.Threading.Tasks;

using Beng.Specta.Compta.Server;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Controllers
{
    public class DashboardControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public DashboardControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task ReturnsViewWithCorrectMessage()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("/", UriKind.Relative));
            
            //VERIFY
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();

            Assert.Contains("FinProd", responseAsString, StringComparison.InvariantCulture);
        }
    }
}
