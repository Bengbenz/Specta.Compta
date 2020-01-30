using System.Threading.Tasks;

namespace Beng.Specta.Compta.SharedKernel.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}