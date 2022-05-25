using Beng.Specta.Compta.Core.Entities.Identities;
using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Identities.Services
{
    /// <summary>
    /// These contain the individual methods to add/update the database, BUT you should call SaveChanges to update the database after using
    /// (This is different to AspNetUserExtension, where the userManger updates the database immediately)
    /// </summary>
    public class IdentityService
    {
        private readonly IAuthorizationRepository _repository;

        public IdentityService(IAuthorizationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// This adds a role if not present, or updates a role if is present.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="permissions"></param>
        public async Task AddOrUpdateRoleToPermissionsAsync(string roleName, string description, params Permission[] permissions)
        {
            IStatusGeneric<RoleToPermissions> status =
                await RoleToPermissions.CreateRoleWithPermissionsAsync(
                    roleName,
                    description,
                    permissions,
                    _repository);
                            
            if (status.IsValid)
            {
                //Note that CreateRoleWithPermissions will return a invalid status if the role is already present.
                await _repository.AddAsync(status.Result!);
            }
            else
            {
                await UpdateRoleAsync(roleName, description, permissions);
            }
        }

        /// <summary>
        /// This will update a role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="permissions"></param>
        public async Task UpdateRoleAsync(
            string roleName,
            string description,
            IReadOnlyCollection<Permission> permissions)
        {
            RoleToPermissions? existingRole = await _repository.GetRoleToPermissionAsync(roleName);
            if (existingRole is null)
            {
                throw new KeyNotFoundException($"Could not find the role {roleName} to update.");
            }
            existingRole.Update(description, permissions);
            await _repository.UpdateAsync(existingRole);
        }

        /// <summary>
        /// This ensures there is a UserToRole linking the userId to the given roleName
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public async Task AddRoleToUserAsync(string userId, string roleName)
        {
            IStatusGeneric<UserToRole> status = await UserToRole.AddRoleToUserAsync(userId, roleName, _repository);
            if (status.IsValid)
            {
                //we assume there is already a link to the role is the status wasn't valid
                await _repository.AddAsync(status.Result!);
            }
        }
    }
}