using System;
using System.Threading.Tasks;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.Core.Handlers;

using Xunit;

namespace Beng.Specta.Compta.UnitTests.Core.Handlers
{
    public class ItemCompletedEmailNotificationHandlerHandle
    {
        [Fact]
        public async Task ThrowsExceptionGivenNullEventArgument()
        {
            var handler = new ItemCompletedEmailNotificationHandler();

            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null));
        }

        [Fact]
        public async Task DoesNothingGivenEventInstance()
        {
            var handler = new ItemCompletedEmailNotificationHandler();

            await handler.Handle(new ToDoItemCompletedEvent(new ToDoItem()));
        }
    }
}
