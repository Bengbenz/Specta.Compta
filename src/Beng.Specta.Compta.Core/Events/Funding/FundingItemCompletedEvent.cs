using Beng.Specta.Compta.Core.Entities.Funding;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

namespace Beng.Specta.Compta.Core.Events.Funding
{
    public class FundingItemCompletedEvent : BaseDomainEvent
    {
        public FundingItem CompletedItem { get; set; }

        public FundingItemCompletedEvent(FundingItem completedItem)
        {
            Guard.Argument(completedItem, nameof(completedItem)).NotNull();
            
            CompletedItem = completedItem;
        }
    }
}