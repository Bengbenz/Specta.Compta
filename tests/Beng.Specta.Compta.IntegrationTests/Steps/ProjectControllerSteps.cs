using Beng.Specta.Compta.Core.Dtos;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Steps;

[Binding]
public class ProjectControllerSteps
{
    private readonly IntegrationTestingWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private HttpResponseMessage? _responseMessage;
    private IReadOnlyCollection<ProjectDto> _result = null!;

    public ProjectControllerSteps(IntegrationTestingWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClientWithFakeAuthorization();
    }
    
    [Given(@"No project was added yet\.")]
    public void GivenNoProjectWasAddedYet()
    {
    }
    
    [When(@"I ask the projects list.")]
    public async Task WhenIAskTheProjectsList()
    {
        _responseMessage = await _client.GetAsync(new Uri("api/project/list", UriKind.Relative));
    }

    [Then(@"The result should be empty\.")]
    public async Task ThenTheResultShouldBeEmpty()
    {
        _responseMessage!.EnsureSuccessStatusCode();
        string responseAsString = await _responseMessage.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<ProjectDto>>(responseAsString);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Given(@"Three projects are already seeded\.")]
    public async Task GivenThreeProjectsAreAlreadySeeded()
    {
        await _factory.Services.AddThreeProjects();
    }

    [Then(@"The result should contain three projects\.")]
    public async Task ThenTheResultShouldContainThreeProjects()
    {
        _responseMessage!.EnsureSuccessStatusCode();
        var responseAsString = await _responseMessage.Content.ReadAsStringAsync();
        _result = JsonConvert.DeserializeObject<IReadOnlyCollection<ProjectDto>>(responseAsString)!;

        Assert.NotNull(_result);
        Assert.NotEmpty(_result!);
        Assert.Equal(3, _result!.Count);
    }

    [Then(@"The first is (.*)")]
    public void ThenTheFirstIs(string code)
    {
        Assert.Equal(code, _result.First().Code);
    }

    [Then(@"The second is (.*)")]
    public void ThenTheSecondIs(string code)
    {
        Assert.Equal(code, _result.ElementAt(1).Code);
    }

    [Then(@"the last is (.*)")]
    public void ThenTheLastIs(string code)
    {
        Assert.Equal(code, _result.Last().Code);
    }
}