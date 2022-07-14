using TechTalk.SpecFlow;

namespace Beng.Specta.Compta.IntegrationTests.Hooks;

[Binding]
public sealed class ProjectControllerHooks
{
    // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

    private readonly IntegrationTestingWebApplicationFactory _factory;
    
    public ProjectControllerHooks(IntegrationTestingWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [BeforeScenario]
    public async Task CleanDatabase()
    {
        await _factory.Services.CleanProjects();
    }
}