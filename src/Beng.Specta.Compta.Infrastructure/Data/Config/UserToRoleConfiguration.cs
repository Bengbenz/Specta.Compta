using Beng.Specta.Compta.Core.Entities.Security.Auth;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public class UserToRoleConfiguration : BaseEntityConfiguration<UserToRole>
    {
        protected override void Configure()
        {
            Builder.HasKey(x => new { x.UserId, x.RoleName });
        }
    }
}