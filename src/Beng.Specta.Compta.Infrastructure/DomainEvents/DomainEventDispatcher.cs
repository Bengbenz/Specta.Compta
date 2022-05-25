using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.Infrastructure.DomainEvents;

// https://gist.github.com/jbogard/54d6569e883f63afebc7
// http://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Dispatch(BaseDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var wrappedHandlers = GetWrappedHandlers(domainEvent);

        foreach (DomainEventHandler handler in wrappedHandlers)
        {
            await handler.Handle(domainEvent).ConfigureAwait(false);
        }
    }

    public IEnumerable<DomainEventHandler> GetWrappedHandlers(BaseDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        Type handlerType = typeof(IHandle<>).MakeGenericType(domainEvent.GetType());
        Type wrapperType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        IEnumerable<object?> handlers = _serviceProvider.GetServices(handlerType);
        IEnumerable<DomainEventHandler> wrappedHandlers = handlers
            .Select(handler => (DomainEventHandler)Activator.CreateInstance(wrapperType, handler)!);

        return wrappedHandlers;
    }
}