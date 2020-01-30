using System.Threading.Tasks;

namespace Beng.Specta.Compta.SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}