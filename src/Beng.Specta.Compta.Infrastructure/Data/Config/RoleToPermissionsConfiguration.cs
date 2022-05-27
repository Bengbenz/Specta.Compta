using Microsoft.EntityFrameworkCore;
using Beng.Specta.Compta.Core.Entities.Identities;

namespace Beng.Specta.Compta.Infrastructure.Data.Config;

public sealed class RoleToPermissionsConfiguration : BaseEntityConfiguration<RoleToPermissions>
{
    protected override void Configure()
    {
        // RoleName is the Key
        Builder.Property("Permissions")
            .HasColumnName("PermissionsInRole");
    }
}