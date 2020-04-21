using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Dawn;

namespace Beng.Specta.Compta.Core.Services.Funding
{
    public class ContractStepCompletedNotificationHandler : IHandle<ContractStepCompletedEvent>
    {
        public Task Handle(ContractStepCompletedEvent domainEvent)
        {
            Guard.Argument(domainEvent, nameof(domainEvent)).NotNull();

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
