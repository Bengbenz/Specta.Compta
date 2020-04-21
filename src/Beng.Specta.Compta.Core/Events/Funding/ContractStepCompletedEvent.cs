using Beng.Specta.Compta.Core.Entities.Funding;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

namespace Beng.Specta.Compta.Core.Events.Funding
{
    public class ContractStepCompletedEvent : BaseDomainEvent
    {
        public ContractStep CompletedStep { get; set; }

        public ContractStepCompletedEvent(ContractStep completedItem)
        {
            Guard.Argument(completedItem, nameof(completedItem)).NotNull();
            
            CompletedStep = completedItem;
        }
    }
}