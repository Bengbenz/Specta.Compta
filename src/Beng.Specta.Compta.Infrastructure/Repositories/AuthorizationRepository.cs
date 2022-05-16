using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities.Identities;
using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Beng.Specta.Compta.Infrastructure.Repositories
{
    public class AuthorizationRepository : EfRepository, IAuthorizationRepository
    {
        public AuthorizationRepository(
            AppDbContext dbContext,
            ILogger<IAuthorizationRepository> logger)
            : base(dbContext, logger)
        {
        }

        public async Task<RoleToPermissions?> GetRoleToPermissionAsync(string roleName)
        {
            return await DbContext.Set<RoleToPermissions>()
                .SingleOrDefaultAsync(o => o.RoleName == roleName);
        }

        public async Task<UserToRole?> GetUserToRoleAsync(string userId, string roleName)
        {
            return await DbContext.Set<UserToRole>()
                .SingleOrDefaultAsync(o => o.UserId == userId && o.RoleName == roleName);
        }

        public async Task<ModulesForUser?> GetModuleForUserAsync(string userId)
        {
            return await DbContext.Set<ModulesForUser>()
                .SingleOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task<IReadOnlyCollection<UserToRole>> GetUsersToRoleByNameAsync(string roleName)
        {
            return await DbContext.Set<UserToRole>()
                .Where(o => o.RoleName == roleName)
                .ToListAsync();
        }
        
        public bool IsAnyUsersWithRole(string roleName)
        {
            bool result = DbContext.Set<UserToRole>().Any(o => o.RoleName == roleName);
            if (result)
            {
                Logger.LogInformation($"{roleName} role is already exist.");
            }

            return result;
        }

        public async Task<IReadOnlyCollection<UserToRole>> GetUsersToRoleByIdAsync(string userId)
        {
            return await DbContext.Set<UserToRole>().Include(x => x.Role)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
