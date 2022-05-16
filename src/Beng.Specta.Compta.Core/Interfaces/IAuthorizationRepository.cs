using System.Collections.Generic;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Entities.Identities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Core.Interfaces
{
    public interface IAuthorizationRepository : IRepository
    {
        Task<RoleToPermissions?> GetRoleToPermissionAsync(string roleName);

        Task<UserToRole?> GetUserToRoleAsync(string userId, string roleName);

        Task<ModulesForUser?> GetModuleForUserAsync(string userId);

        Task<IReadOnlyCollection<UserToRole>> GetUsersToRoleByNameAsync(string roleName);

        bool IsAnyUsersWithRole(string roleName);

        Task<IReadOnlyCollection<UserToRole>> GetUsersToRoleByIdAsync(string userId);
    }
}