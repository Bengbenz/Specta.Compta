using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Events
{
    public class ToDoItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem CompletedItem { get; set; }

        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}