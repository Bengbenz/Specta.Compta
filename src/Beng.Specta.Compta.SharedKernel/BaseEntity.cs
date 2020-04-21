using System.Collections.Generic;

namespace Beng.Specta.Compta.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public ICollection<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}