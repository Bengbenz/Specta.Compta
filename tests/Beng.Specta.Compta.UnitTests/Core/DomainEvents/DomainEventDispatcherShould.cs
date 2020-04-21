using Autofac;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.Infrastructure;
using Beng.Specta.Compta.Infrastructure.DomainEvents;
using Xunit;

namespace Beng.Specta.Compta.UnitTests.Core.DomainEvents
{
    public class DomainEventDispatcherShould
    {
        [Fact]
        public void NotReturnAnEmptyListOfAvailableHandlers()
        {
            IContainer container = ServiceCollectionExtentions.BaseAutofacInitialization();

            var domainEventDispatcher = new DomainEventDispatcher(container);
            var toDoItemCompletedEvent = new ToDoItemCompletedEvent(new ToDoItem());

            var handlersForEvent = domainEventDispatcher.GetWrappedHandlers(toDoItemCompletedEvent);

            Assert.NotEmpty(handlersForEvent);
        }
    }
}
