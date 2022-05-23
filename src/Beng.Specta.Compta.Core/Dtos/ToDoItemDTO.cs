using System;
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.Dtos
{
    // Note: doesn't expose events or behavior
    public class ToDoItemDTO : BaseDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public static ToDoItemDTO FromToDoItem(ToDoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

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
