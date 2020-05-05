using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Beng.Specta.Compta.SharedKernel;

using Finbuckle.MultiTenant.EntityFrameworkCore;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        protected EntityTypeBuilder<T> Builder { get; private set; }

        protected abstract void Configure();

        public void Configure(EntityTypeBuilder<T> builder)
        {
            Builder = builder;
            builder.IsMultiTenant();

            Configure();
        }
    }
}
