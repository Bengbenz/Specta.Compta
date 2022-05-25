using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Infrastructure.DomainEvents;

public abstract class DomainEventHandler
{
    public abstract Task Handle(BaseDomainEvent domainEvent);
}