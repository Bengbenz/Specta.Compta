using System.Collections.Generic;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Entities.Security.Auth;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Core.Interfaces
{
    public interface IAuthorizationRepository : IRepository
    {
        Task<RoleToPermissions> GetRoleToPermissionAsync(string roleName);

        Task<UserToRole> GetUserToRoleAsync(string userId, string roleName);

        Task<ModulesForUser> GetModuleForUserAsync(string userId);

        Task<ICollection<UserToRole>> GetUsersToRoleByNameAsync(string roleName);

        Task<ICollection<UserToRole>> GetUsersToRoleByIdAsync(string userId);
    }
}