using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Core.Handlers;

public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
{
    public Task Handle(ToDoItemCompletedEvent domainEvent)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        // Do Nothing
        return Task.CompletedTask;
    }
}