using System.Reflection;
using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Identities;

namespace Beng.Specta.Compta.Server.Identities;

/// <summary>
/// This is the code that calculates what feature permissions a user has
/// </summary>
public class FeaturePermissionsBuilder
{
    private readonly IAuthorizationRepository _repository;

    public FeaturePermissionsBuilder(IAuthorizationRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// This is called if the Permissions that a user needs calculating.
    /// It looks at what permissions the user has, and then filters out any permissions
    /// they aren't allowed because they haven't get access to the module that permission is linked to.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>a string containing the packed permissions</returns>
    public async Task<string> ComputeUserPermissionsAsync(string userId)
    {
        // This gets all the permissions, with a distinct to remove duplicates
        var permissionsForUser = (await _repository.GetUsersToRoleByIdAsync(userId))
            .Select(x => x.Role)
            //Because the permissions are packed we have to put these parts of the query after the ToListAsync()
            .SelectMany(x => x.PermissionsInRole).Distinct();

        //we get the modules this user is allowed to see
        PaidForModule userModules = (await _repository.GetModuleForUserAsync(userId))?.AllowedModules ?? PaidForModule.None;
        //Now we remove permissions that are linked to modules that the user has no access to
        var filteredPermissions =
            from permission in permissionsForUser
            let moduleAttr = typeof(Permission).GetMember(permission.ToString())[0]
                .GetCustomAttribute<LinkedToModuleAttribute>()
            where moduleAttr == null || userModules.HasFlag(moduleAttr.PaidForModule)
            select permission;

        return filteredPermissions.PackPermissionsIntoString();
    }

}