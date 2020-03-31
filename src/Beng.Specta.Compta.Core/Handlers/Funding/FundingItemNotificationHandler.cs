using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Dawn;

namespace Beng.Specta.Compta.Core.Services.Funding
{
    public class FundingItemNotificationHandler : IHandle<FundingItemCompletedEvent>
    {
        public Task Handle(FundingItemCompletedEvent domainEvent)
        {
            Guard.Argument(domainEvent, nameof(domainEvent)).NotNull();

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
