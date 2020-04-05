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
                            .Select(ToDoItemDTO.FromToDoItem);
            return Ok(items);
        }

        // GET: api/Projects
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = ToDoItemDTO.FromToDoItem(_repository.GetById<Project>(id));
            return Ok(item);
        }

        // POST: api/Projects
        [HttpPost]
        public IActionResult Post([FromBody] ToDoItemDTO item)
        {
            var todoItem = new Project()
            {
                Title = item.Title,
                Description = item.Description
            };
            _repository.Add(todoItem);
            return Ok(ToDoItemDTO.FromToDoItem(todoItem));
        }

        [HttpPatch("{id:int}/complete")]
        public IActionResult Complete(int id)
        {
            var toDoItem = _repository.GetById<ToDoItem>(id);
            toDoItem.MarkComplete();
            _repository.Update(toDoItem);

            return Ok(ToDoItemDTO.FromToDoItem(toDoItem));
        }
    }
}
