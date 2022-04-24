using Beng.Specta.Compta.Core.Config;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.Infrastructure.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Beng.Specta.Compta.UnitTests.Core.DomainEvents
{
    public class DomainEventDispatcherShould
    {
        [Fact]
        public void NotReturnAnEmptyListOfAvailableHandlers()
        {
            // Arrange
            IServiceCollection container = new ServiceCollection();
            container.AddAppCoreServices();

            var domainEventDispatcher = new DomainEventDispatcher(container.BuildServiceProvider());
            var toDoItemCompletedEvent = new ToDoItemCompletedEvent(new ToDoItem());

            // Act
            var handlersForEvent = domainEventDispatcher.GetWrappedHandlers(toDoItemCompletedEvent);

            // Assert
            Assert.NotEmpty(handlersForEvent);
        }
    }
}
