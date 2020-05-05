using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities.Security.Auth;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public class RoleToPermissionsConfiguration : BaseEntityConfiguration<RoleToPermissions>
    {
        protected override void Configure()
        {
            Builder.Property("_permissionsInRole")
                .HasColumnName("PermissionsInRole");
        }
    }
}