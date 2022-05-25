using System.ComponentModel.DataAnnotations.Schema;

namespace Beng.Specta.Compta.SharedKernel;

// This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
public abstract class BaseEntity
{
    public long Id { get; set; }

    // Events (event-driven pattern). Don't need to make them as Properties otherwise, EF persist them in DB
    [NotMapped]
    public ICollection<BaseDomainEvent> Events { get; private set; } = new List<BaseDomainEvent>();
}