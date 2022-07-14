using System.Net.Http.Headers;
using Beng.Specta.Compta.Core.Dtos;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Steps;

[Binding]
public sealed class ProjectControllerSteps
{
    private readonly IntegrationTestingWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private HttpResponseMessage? _responseMessage;
    private IReadOnlyCollection<ProjectDto> _result = null!;
    private ProjectDto _project = null!;

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

    [Given(@"I give a minimum information about a project\.")]
    public void GivenIGiveAMinimumInformationAboutAProject()
    {
        _project = new ProjectDto
        {
            Code = "DTS",
            Name = "Desktop Provider",
            StartDate = DateTime.Now,
            Duration = 2
        };
    }

    [When(@"I save this project by web api\.")]
    public async Task WhenISaveThisProjectByWebApi()
    {
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(_project));
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        _responseMessage = await _client.PostAsync(new Uri("api/project/post", UriKind.Relative), httpContent);
    }

    [Then(@"The project is saved in DB and returned\.")]
    public async Task ThenTheProjectIsSavedInDbAndReturned()
    {
        _responseMessage!.EnsureSuccessStatusCode();
        var responseAsString = await _responseMessage.Content.ReadAsStringAsync();
        var newItem = JsonConvert.DeserializeObject<ProjectDto>(responseAsString)!;
        
        Assert.NotNull(newItem);
        Assert.True(newItem.Id > 0);
        Assert.Equal(_project.Code, newItem.Code);
        Assert.Equal(_project.Name, newItem.Name);
        Assert.Equal(_project.Description, newItem.Description);
        Assert.Equal(_project.StartDate, newItem.StartDate);
    }
}