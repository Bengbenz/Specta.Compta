using Beng.Specta.Compta.Core.Entities.Funding;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Events.Funding;

public sealed class ContractStepCompletedEvent : BaseDomainEvent
{
    public ContractStep CompletedStep { get; set; }

    public ContractStepCompletedEvent(ContractStep completedItem)
    {
        CompletedStep = completedItem ?? throw new ArgumentNullException(nameof(completedItem));
    }
}