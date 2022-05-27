using Beng.Specta.Compta.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Finbuckle.MultiTenant.EntityFrameworkCore;

namespace Beng.Specta.Compta.Infrastructure.Data.Config;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    protected EntityTypeBuilder<T> Builder { get; private set; } = null!;

    protected virtual void Configure()
    {
        // To override Key config, don't call this base method
        Builder.HasKey(x => x.Id);
    }

    public void Configure(EntityTypeBuilder<T> builder)
    {
        Builder = builder;
        builder.IsMultiTenant();
        
        Configure();
    }
}