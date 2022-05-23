using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Controllers
{
    public class DashboardControllerTest : IClassFixture<IntegrationTestingWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DashboardControllerTest(IntegrationTestingWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsViewWithCorrectMessage()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("/", UriKind.Relative));
            
            // Assert
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();

            Assert.Contains("FinProd", responseAsString, StringComparison.InvariantCulture);
        }
    }
}
