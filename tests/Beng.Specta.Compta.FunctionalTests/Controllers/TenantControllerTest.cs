using System;
using System.Net.Http;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Server;

using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Controllers
{
    public class TenantControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TenantControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task Info_ReturnsVersionAndLastUpdateDate()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/tenant/info", UriKind.Relative));
            
            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Version", stringResponse, StringComparison.InvariantCulture);
            Assert.Contains("Last Updated", stringResponse, StringComparison.InvariantCulture);
        }

        [Fact]
        public async Task CurrentTenant_ReturnsTenantInfo()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(new Uri("api/tenant/currenttenant", UriKind.Relative));
            
            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TenantInfoDTO>(stringResponse);
            
            Assert.NotNull(result);
            Assert.Equal("finprod", result.Identifier);
            Assert.Equal("default-tenant-001", result.Id);
            Assert.NotEmpty(result.Name);
        }
    }
}
