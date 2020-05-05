// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Entities.Security.Permissions;
using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.SharedKernel.Status;

namespace Beng.Specta.Compta.Core.Entities.Security.Auth
{
    /// <summary>
    /// This holds each Roles, which are mapped to Permissions
    /// </summary>
    public class RoleToPermissions : BaseEntity
    {
        [Required(AllowEmptyStrings = false)] //A role must have at least one role in it
        private string _permissionsInRole;

        private RoleToPermissions() { }

        /// <summary>
        /// This creates the Role with its permissions
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="permissions"></param>
        private RoleToPermissions(string roleName, string description, IEnumerable<Permission> permissions)
        {
            RoleName = roleName;
            Update(description, permissions);
        }

        /// <summary>
        /// ShortName of the role
        /// </summary>
        [Key] // TODO: Unique instead of Key
        [Required(AllowEmptyStrings = false)]
        [MaxLength(AuthConstants.RoleNameSize)]
        public string RoleName { get; private set; }

        /// <summary>
        /// A human-friendly description of what the Role does
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Description { get; private set; }

        /// <summary>
        /// This returns the list of permissions in this role
        /// </summary>
        public IEnumerable<Permission> PermissionsInRole => _permissionsInRole.UnpackPermissionsFromString();

        public static async Task<IStatusGeneric<RoleToPermissions>> CreateRoleWithPermissionsAsync(
            string roleName,
            string description,
            ICollection<Permission> permissionInRole,
            IAuthorizationRepository repository)
        {
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            var status = new StatusGenericHandler<RoleToPermissions>();
            RoleToPermissions roleToPermissions = await repository.GetRoleToPermissionAsync(roleName);
            if (roleToPermissions != null)
            {
                status.AddError("That role already exists");
                return status;
            }

            return status.SetResult(new RoleToPermissions(roleName, description, permissionInRole));
        }

        public void Update(string description, IEnumerable<Permission> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                throw new InvalidOperationException("There should be at least one permission associated with a role.");
            }

            _permissionsInRole = permissions.PackPermissionsIntoString();
            Description = description;
        }

        public async Task<IStatusGeneric> DeleteRoleAsync(
            string roleName,
            bool isRemoveFromUsers,
            IAuthorizationRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            var status = new StatusGenericHandler { Message = "Deleted role successfully." };
            RoleToPermissions roleToUpdate = await repository.GetRoleToPermissionAsync(roleName);
            if (roleToUpdate == null)
            {
                return status.AddError("That role doesn't exists");
            }

            ICollection<UserToRole> usersWithRoles = await repository.GetUsersToRoleByNameAsync(roleName);
            if (usersWithRoles.Any())
            {
                if (!isRemoveFromUsers)
                {
                    return status.AddError($"That role is used by {usersWithRoles.Count} and you didn't ask for them to be updated.");
                }

                await repository.DeleteRangeAsync(usersWithRoles);
                status.Message = $"Removed role from {usersWithRoles.Count} user and then deleted role successfully.";
            }

            await repository.DeleteAsync(roleToUpdate);
            return status;
        }
    }
}