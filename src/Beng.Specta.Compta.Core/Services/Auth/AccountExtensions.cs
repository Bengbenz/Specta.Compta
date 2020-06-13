using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Beng.Specta.Compta.Core.Objects.Auth;

namespace Beng.Specta.Compta.Core.Services.Auth
{
    public static class AccountExtensions
    {
        /// <summary>
        /// This gets the permissions for the currently logged in user (or null if no claim)
        /// </summary>
        /// <param name="usersClaims"></param>
        /// <returns></returns>
        public static IEnumerable<Permission> PermissionsFromClaims(this IEnumerable<Claim> usersClaims)
        {
            if (usersClaims == null) throw new ArgumentNullException(nameof(usersClaims));

            var permissionsClaim =
                usersClaims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
            return permissionsClaim.Value.UnpackPermissionsFromString();
        }

        public static string Email(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
        
        public static string Name(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}