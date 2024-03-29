﻿using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.SharedKernel.Status;

namespace Beng.Specta.Compta.Core.Entities.Identities;

/// <summary>
/// This holds each Roles, which are mapped to Permissions
/// </summary>
public sealed class RoleToPermissions : BaseEntity
{
    private RoleToPermissions() { } //needed by EF Core

    /// <summary>
    /// This creates the Role with its permissions
    /// </summary>
    /// <param name="roleName">ShortName of the role</param>
    /// <param name="description">A human-friendly description of what the Role does</param>
    /// <param name="permissions">The list of permissions in this role</param>
    private RoleToPermissions(
        string roleName,
        string description,
        IReadOnlyCollection<Permission> permissions)
    {
        RoleName = roleName;
        Update(description, permissions);
    }

    /// <summary>
    /// ShortName of the role
    /// </summary>
    [Key]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(IdentityConstants.RoleNameSize)]
    public string RoleName { get; private set; } = null!;

    /// <summary>
    /// A human-friendly description of what the Role does
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Description { get; private set; } = null!;

    [Required(AllowEmptyStrings = false)] //A role must have at least one role in it
    private string Permissions { get; set; } = null!;

    /// <summary>
    /// This returns the list of permissions in this role
    /// </summary>
    public IEnumerable<Permission> PermissionsInRole => Permissions.UnpackPermissionsFromString();

    public static async Task<IStatusGeneric<RoleToPermissions>> CreateRoleWithPermissionsAsync(
        string roleName,
        string description,
        IReadOnlyCollection<Permission> permissionInRole,
        IAuthorizationRepository repository)
    {
        ArgumentNullException.ThrowIfNull(roleName);
        ArgumentNullException.ThrowIfNull(repository);

        var status = new StatusGenericHandler<RoleToPermissions>();
        RoleToPermissions? roleToPermissions = await repository.GetRoleToPermissionAsync(roleName);
        if (roleToPermissions is not null)
        {
            status.AddError("That role already exists");
            return status;
        }

        return status.SetResult(new RoleToPermissions(roleName, description, permissionInRole));
    }

    public void Update(
        string description,
        IReadOnlyCollection<Permission> permissions)
    {
        ArgumentNullException.ThrowIfNull(permissions);
        if (!permissions.Any())
        {
            throw new ArgumentException($"{nameof(permissions)} is empty. There should be at least one permission associated with a role.");
        }

        Permissions = permissions.PackPermissionsIntoString();
        Description = description;
    }

    public async Task<IStatusGeneric> DeleteRoleAsync(
        string roleName,
        bool isRemoveFromUsers,
        IAuthorizationRepository repository)
    {
        if (repository == null) throw new ArgumentNullException(nameof(repository));

        var status = new StatusGenericHandler { Message = "Deleted role successfully." };
        RoleToPermissions? roleToUpdate = await repository.GetRoleToPermissionAsync(roleName);
        if (roleToUpdate == null)
        {
            return status.AddError("That role doesn't exists");
        }

        IReadOnlyCollection<UserToRole> usersWithRoles = await repository.GetUsersToRoleByNameAsync(roleName);
        if (usersWithRoles.Any())
        {
            if (!isRemoveFromUsers)
            {
                return status.AddError($"That role is used by {usersWithRoles.Count} and you didn't ask for them to be updated.");
            }

            await repository.DeleteAsync(usersWithRoles);
            status.Message = $"Removed role from {usersWithRoles.Count} user and then deleted role successfully.";
        }

        await repository.DeleteAsync(roleToUpdate);
        return status;
    }
}