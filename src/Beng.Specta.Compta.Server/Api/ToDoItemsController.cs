using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Api
{
    public class ToDoItemsController : BaseApiController
    {
        private readonly IRepository _repository;

        public ToDoItemsController(IRepository repository, ILogger<ToDoItemsController> logger) : base(logger)
        {
            _repository = repository;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
            //var items = _repository.List<ToDoItem>()
            //                .Select(ToDoItemDTO.FromToDoItem);
            return Ok();
        }

        // GET: api/ToDoItems
        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            var item = ToDoItemDTO.FromToDoItem(_repository.GetById<ToDoItem>(id));
            return Ok(item);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public IActionResult Post([FromBody] ToDoItemDTO item)
        {
            var todoItem = new ToDoItem()
            {
                Title = item.Title,
                Description = item.Description
            };
            _repository.Add(todoItem);
            return Ok(ToDoItemDTO.FromToDoItem(todoItem));
        }

        [HttpPatch("{id:long}/complete")]
        public IActionResult Complete(long id)
        {
            var toDoItem = _repository.GetById<ToDoItem>(id);
            toDoItem.MarkComplete();
            _repository.Update(toDoItem);

            return Ok(ToDoItemDTO.FromToDoItem(toDoItem));
        }
    }
}
