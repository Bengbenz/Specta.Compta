using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.DTOs
{
    // Note: doesn't expose events or behavior
    public class ToDoItemDTO : BaseDTO
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public static ToDoItemDTO FromToDoItem(ToDoItem item)
        {
            return new ToDoItemDTO
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone
            };
        }
    }
}
