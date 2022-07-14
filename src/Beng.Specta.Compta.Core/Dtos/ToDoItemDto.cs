using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.Dtos;

// Note: doesn't expose events or behavior
public class ToDoItemDto : BaseDto
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; private set; }

    private ToDoItemDto(string title)
    {
        Title = title;
    }

    public static ToDoItemDto FromToDoItem(ToDoItem? item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new ToDoItemDto(item.Title)
        {
            Id = item.Id,
            Description = item.Description,
            IsDone = item.IsDone
        };
    }
}