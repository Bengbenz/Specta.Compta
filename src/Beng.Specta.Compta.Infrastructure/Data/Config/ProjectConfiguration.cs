using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Infrastructure.Data.Config;

public sealed class ProjectConfiguration : BaseEntityConfiguration<Project>
{
    protected override void Configure()
    {
        // Id is a key
        base.Configure();
        Builder.HasIndex(p => p.Code).IsUnique();
        Builder.HasIndex(p => p.Name).IsUnique();
    }
}