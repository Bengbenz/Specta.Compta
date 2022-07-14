using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Beng.Specta.Compta.Core.Dtos;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Controllers;

//[HasPermission(Permission.ProjectAllAccess)]
[Authorize]
public class ProjectController : BaseApiController
{
    private readonly IRepository _repository;

    public ProjectController(
        IRepository repository,
        ILogger<ProjectController> logger) : base(logger)
    {
        _repository = repository;
    }

    // GET: api/Project
    [HttpGet, AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var projectDtos = (await _repository.ListAsync<Project>()).Select(x => x.ToDto()).ToList();
        return Ok(projectDtos);
    }

    // GET: api/Project
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var projectDto = (await _repository.GetByIdAsync<Project>(id))?.ToDto();
        return Ok(projectDto);
    }

    // POST: api/Project
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] ProjectDto projectDto)
    {
        ArgumentNullException.ThrowIfNull(projectDto);

        var projectItem = new Project(projectDto.Code, projectDto.Name)
        {
            Description = projectDto.Description,
            StartDate = projectDto.StartDate,
            Duration = projectDto.Duration
        };

        await _repository.AddAsync(projectItem);
        return Ok(projectItem.ToDto()); // CreatedAtAction(nameof(projectItem.Code), projectItem.ToDTO());
    }

    /*
    [HttpPatch("{id:long}/complete")]
    public IActionResult Complete(long id)
    {
        var projectItem = _repository.GetById<Project>(id);
        projectItem.MarkComplete();
        _repository.Update(projectItem);

        return Ok(projectItem.ToDTO());
    }*/
}