using System.Linq;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.Server.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Beng.Specta.Compta.Server.Api
{
    public class ToDoItemsController : BaseApiController
    {
        private readonly IRepository _repository;

        public ToDoItemsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
            var items = _repository.List<ToDoItem>()
                            .Select(ToDoItemDTO.FromToDoItem);
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
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
