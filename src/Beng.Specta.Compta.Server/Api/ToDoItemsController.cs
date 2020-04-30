using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Dawn;

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
        public ActionResult<IEnumerable<ToDoItemDTO>> List()
        {
            IEnumerable<ToDoItemDTO> items = _repository.List<ToDoItem>()
                                                        .Select(ToDoItemDTO.FromToDoItem);
            Logger.LogInformation(string.Join(", ", items.Select(x => x.Title)));
            return items.ToList();
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
            Guard.Argument(item, nameof(item)).NotNull();

            var todoItem = new ToDoItem
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
