using Beng.Specta.Compta.Core.Entities.Identities;

namespace Beng.Specta.Compta.Infrastructure.Data.Config;

public sealed class UserToRoleConfiguration : BaseEntityConfiguration<UserToRole>
{
    protected override void Configure()
    {
        Builder.HasKey(x => new { x.UserId, x.RoleName });
    }
}