using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.SharedKernel.Status;

namespace Beng.Specta.Compta.Core.Entities.Identities;

/// <summary>
/// This is a one-to-many relationship between the User (represented by the UserId) and their Roles (represented by RoleToPermissions)
/// </summary>
public sealed class UserToRole : BaseEntity
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
    [MaxLength(IdentityConstants.UserIdSize)] 
    public string UserId { get; private set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(IdentityConstants.RoleNameSize)]
    public string RoleName { get; private set; } = null!;

    [ForeignKey(nameof(RoleName))]
    public RoleToPermissions Role { get; set; } = null!;

    public static async Task<IStatusGeneric<UserToRole>> AddRoleToUserAsync(
        string userId,
        string roleName,
        IAuthorizationRepository repository)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(roleName);
        ArgumentNullException.ThrowIfNull(repository);

        var status = new StatusGenericHandler<UserToRole>();
        UserToRole? userToRole = await repository.GetUserToRoleAsync(userId, roleName);
        if (userToRole is not null)
        {
            status.AddError($"The user already has the Role '{roleName}'.");
            return status;
        }

        RoleToPermissions? roleToAdd = await repository.GetRoleToPermissionAsync(roleName);
        if (roleToAdd is null)
        {
            status.AddError($"Could not find the Role '{roleName}'.");
            return status;
        }

        return status.SetResult(new UserToRole(userId, roleToAdd));
    }
}