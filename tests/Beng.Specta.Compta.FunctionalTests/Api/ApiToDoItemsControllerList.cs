using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Server;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Xunit;

namespace Beng.Specta.Compta.FunctionalTests.Api
{
    public class ApiToDoItemsControllerList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApiToDoItemsControllerList(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(Skip = "To fix later")]
        public async Task ReturnsThreeItems()
        {
            HttpResponseMessage response = await _client.GetAsync("api/todoitems");
            response.EnsureSuccessStatusCode();
            string stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ToDoItem>>(stringResponse).ToList();

            Assert.Equal(3, result.Count());
        }
    }
}
