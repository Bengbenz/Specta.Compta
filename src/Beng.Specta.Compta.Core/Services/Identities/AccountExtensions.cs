using System.Security.Claims;

using Beng.Specta.Compta.Core.Objects.Identities;

namespace Beng.Specta.Compta.Core.Services.Identities;

public static class AccountExtensions
{
    /// <summary>
    /// This gets the permissions for the currently logged in user (or null if no claim)
    /// </summary>
    /// <param name="usersClaims"></param>
    /// <returns></returns>
    public static IEnumerable<Permission> PermissionsFromClaims(this IEnumerable<Claim> usersClaims)
    {
        ArgumentNullException.ThrowIfNull(usersClaims);

        var permissionsClaim =
            usersClaims.Single(c => c.Type == PermissionConstants.PackedPermissionClaimType);
        return permissionsClaim.Value.UnpackPermissionsFromString();
    }
    
    public static string PermissionAsString(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.Claims.Single(c => c.Type == PermissionConstants.PackedPermissionClaimType).Value;
    }

    public static string? Email(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.FindFirst(ClaimTypes.Email)?.Value;
    }
        
    public static string? Name(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }
}