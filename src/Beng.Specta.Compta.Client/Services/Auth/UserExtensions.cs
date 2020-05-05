using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Beng.Specta.Compta.Core.Entities.Security.Permissions;
using Beng.Specta.Compta.Core.Utils.Auth;

namespace Beng.Specta.Compta.Client.Services.Auth
{
    public static class UserExtensions
    {
        /// <summary>
        /// This gets the permissions for the currently logged in user (or null if no claim)
        /// </summary>
        /// <param name="usersClaims"></param>
        /// <returns></returns>
        public static IEnumerable<Permission> PermissionsFromClaims(this IEnumerable<Claim> usersClaims)
        {
            var permissionsClaim =
                usersClaims?.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
            return permissionsClaim?.Value.UnpackPermissionsFromString();
        }
    }
}