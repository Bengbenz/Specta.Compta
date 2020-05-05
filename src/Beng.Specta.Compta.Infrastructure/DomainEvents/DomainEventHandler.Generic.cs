using System.Threading.Tasks;

using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Infrastructure.DomainEvents
{
    public class DomainEventHandler<T> : DomainEventHandler
        where T : BaseDomainEvent
    {
        private readonly IHandle<T> _handler;

        public DomainEventHandler(IHandle<T> handler)
        {
            _handler = handler;
        }

        public override Task Handle(BaseDomainEvent domainEvent)
        {
            return _handler.Handle((T)domainEvent);
        }
    }
}