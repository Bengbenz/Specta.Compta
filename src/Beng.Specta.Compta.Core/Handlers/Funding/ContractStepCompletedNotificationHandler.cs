using System;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Core.Handlers.Funding
{
    public class ContractStepCompletedNotificationHandler : IHandle<ContractStepCompletedEvent>
    {
        public Task Handle(ContractStepCompletedEvent domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
