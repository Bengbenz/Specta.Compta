﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Beng.Specta.Compta.Core.Dtos;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Controllers
{
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<ProjectDTO>> List()
        {
            return (await _repository.ListAsync<Project>()).Select(x => x.ToDTO()).ToList();
        }

        // GET: api/Project
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = (await _repository.GetByIdAsync<Project>(id))?.ToDTO();
            return Ok(item);
        }

        // POST: api/Projects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post([FromBody] ProjectDTO item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var projectItem = new Project
            {
                Code = item.Code,
                Name = item.Name,
                Description = item.Description,
                StartDate = item.StartDate,
                Duration = item.Duration
            };

            await _repository.AddAsync(projectItem);
            return Ok(projectItem.ToDTO()); // CreatedAtAction(nameof(projectItem.Code), projectItem.ToDTO());
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
}
