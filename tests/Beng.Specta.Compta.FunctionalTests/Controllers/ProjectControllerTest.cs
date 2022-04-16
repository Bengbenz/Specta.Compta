using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Controllers
{
    public class ProjectControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory?.CreateClientWithoutAuthorization();
        }

        [Fact]
        public async Task List_ReturnsEmptyItems()
        {
            //SETUP

            //ATTEMPT
            var response = await _client.GetAsync(new Uri("api/project/list", UriKind.Relative));
            
            //VERIFY
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ProjectDTO>>(responseAsString);

            Assert.Empty(result);
        }
    }
}
