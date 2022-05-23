using Xunit;
using Xunit.Abstractions;

namespace Beng.Specta.Compta.FunctionalTests.Client.Pages
{
    public class LoginPage_Should : IClassFixture<FunctionalTestingWebApplicationFactory>
    {
        private readonly FunctionalTestingWebApplicationFactory _factory; 
        
        public LoginPage_Should (
            FunctionalTestingWebApplicationFactory factory,
            ITestOutputHelper testOutputHelper)
        {
            _factory = factory
                .WithTestOutputHelper(testOutputHelper)
                .CreateDefaultClient();
        }
        
        [Theory]
        [ClassData(typeof(BrowsersTestData))]
        public async Task Show_LoginForm(string browserType, string browserChannel)
        {
            // Configure the options to use with the fixture for this test
            await _factory.WithFixtureOptions(browserType, browserChannel)
                .WithPageAsync(async page =>
                {
                    // Open the index page => login page
                    await page.GotoAsync(_factory.BaseUrl);
                    await page.WaitForLoadStateAsync();

                    // Wait for the login page redirect to load
                    await page.WaitForSelectorAsync("div[role=\"presentation\"]:has-text(\"Se connecter\")");
                    Assert.Equal($"{_factory.BaseUrl}/login", page.Url);
                    
                    // Click [aria-label="Email"]
                    await page.Locator("[aria-label=\"Email\"]").ClickAsync();
                    // Press [aria-label="Mot de passe"]
                    await page.Locator("[aria-label=\"Mot de passe\"]").ClickAsync();
                    
                    // Click button:has-text("Se connecter")
                    await page.Locator("button:has-text(\"Se connecter\")").ClickAsync();
                    
                    await page.WaitForLoadStateAsync();
                    
                    Assert.Contains("The Email field is required", await page.ContentAsync());
                    Assert.Contains("The Password field is required", await page.ContentAsync());
                });
        }
    }
}