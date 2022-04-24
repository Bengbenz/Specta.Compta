using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities.Auth;
using Beng.Specta.Compta.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Beng.Specta.Compta.Infrastructure.Data.Repositories
{
    public class AuthorizationRepository : EfRepository, IAuthorizationRepository
    {
        public AuthorizationRepository(
            AppDbContext dbContext,
            ILogger<AuthorizationRepository> logger = null)
            : base(dbContext, logger)
        {
        }

        public async Task<RoleToPermissions> GetRoleToPermissionAsync(string roleName)
        {
            return await DbContext.Set<RoleToPermissions>()
                .SingleOrDefaultAsync(o => o.RoleName == roleName);
        }

        public async Task<UserToRole> GetUserToRoleAsync(string userId, string roleName)
        {
            return await DbContext.Set<UserToRole>()
                .SingleOrDefaultAsync(o => o.UserId == userId && o.RoleName == roleName);
        }

        public async Task<ModulesForUser> GetModuleForUserAsync(string userId)
        {
            return await DbContext.Set<ModulesForUser>()
                .SingleOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task<ICollection<UserToRole>> GetUsersToRoleByNameAsync(string roleName)
        {
            return await DbContext.Set<UserToRole>()
                .Where(o => o.RoleName == roleName)
                .ToListAsync();
        }
        
        public bool IsAnyUsersWithRole(string roleName)
        {
            return DbContext.UserToRoles.Any(o => o.RoleName == roleName);
        }

        public async Task<ICollection<UserToRole>> GetUsersToRoleByIdAsync(string userId)
        {
            return await DbContext.Set<UserToRole>()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
