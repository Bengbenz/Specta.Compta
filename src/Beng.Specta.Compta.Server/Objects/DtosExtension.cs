using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using Beng.Specta.Compta.Core.DTOs;

namespace Beng.Specta.Compta.Server.Objects
{
    public static class DtosExtension
    {
        public static UserInfoDto ToDto(this IdentityUser user, ClaimsPrincipal principalClaims)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (principalClaims == null) throw new ArgumentNullException(nameof(principalClaims));

            return new UserInfoDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                HasPassword = !string.IsNullOrEmpty(user.PasswordHash),
                // Title = user.Title
                IsAuthenticated = principalClaims.Identity!.IsAuthenticated,
                // RoleNames = 
                // PackedPermissions = 
                // IsAdmin = user.Roles.Contains(ApplicationUser.AdminRole),
            };
        }
    }
}
