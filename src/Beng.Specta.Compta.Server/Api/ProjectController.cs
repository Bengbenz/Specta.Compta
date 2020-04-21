using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Api
{
    public class ProjectController : BaseApiController
    {
        private readonly IRepository _repository;

        public ProjectController(
            IRepository repository,
            ILogger<ProjectController> logger) : base(logger)
        {
            _repository = repository;
        }

        // GET: api/Projects
        [HttpGet]
        public IActionResult List()
        {
            var items = _repository.List<Project>()
                            .Select(x => x.ToDTO());
            return Ok(items);
        }

        // GET: api/Projects
        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            var item = _repository.GetById<Project>(id)?.ToDTO();
            return Ok(item);
        }

        // POST: api/Projects
        [HttpPost]
        public IActionResult Post([FromBody] ProjectDTO item)
        {
            var projectItem = new Project
            {
                Code = item.Code,
                Name = item.Name,
                Description = item.Description,
                StartDate = item.StartDate,
                Duration = item.Duration
            };

            _repository.Add(projectItem);
            return Ok(projectItem.ToDTO());
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
