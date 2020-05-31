using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Auth;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.SharedKernel.Status;

namespace Beng.Specta.Compta.Core.Entities.Auth
{
    /// <summary>
    /// This is a one-to-many relationship between the User (represented by the UserId) and their Roles (represented by RoleToPermissions)
    /// </summary>
    public class UserToRole
    {
        private UserToRole() { } //needed by EF Core

        public UserToRole(string userId, RoleToPermissions role)
        {
            UserId = userId;
            Role = role;
        }

        //I use a composite key for this table: combination of UserId and RoleName
        //That has to be defined by EF Core's fluent API
        [Required(AllowEmptyStrings = false)]
        [MaxLength(AuthConstants.UserIdSize)] 
        public string UserId { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(AuthConstants.RoleNameSize)]
        public string RoleName { get; private set; }

        [ForeignKey(nameof(RoleName))]
        public RoleToPermissions Role { get; private set; }

        public static async Task<IStatusGeneric<UserToRole>> AddRoleToUserAsync(
            string userId,
            string roleName,
            IAuthorizationRepository repository)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            var status = new StatusGenericHandler<UserToRole>();
            UserToRole userToRole = await repository.GetUserToRoleAsync(userId, roleName);
            if (userToRole != null)
            {
                status.AddError($"The user already has the Role '{roleName}'.");
                return status;
            }

            RoleToPermissions roleToAdd = await repository.GetRoleToPermissionAsync(roleName);
            if (roleToAdd == null)
            {
                status.AddError($"Could not find the Role '{roleName}'.");
                return status;
            }

            return status.SetResult(new UserToRole(userId, roleToAdd));
        }
    }
}