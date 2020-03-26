using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Dawn;

namespace Beng.Specta.Compta.Core.Services
{
    public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
    {
        public Task Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Argument(domainEvent, nameof(domainEvent)).NotNull();

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
