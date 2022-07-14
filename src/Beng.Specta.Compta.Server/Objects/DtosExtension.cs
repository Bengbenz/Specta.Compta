using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Beng.Specta.Compta.Core.Dtos.Identities;
using Beng.Specta.Compta.Core.Services.Identities;

namespace Beng.Specta.Compta.Server.Objects;

public static class DtosExtension
{
    public static UserInfoDto ToDto(this IdentityUser user, ClaimsPrincipal principalClaims)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(principalClaims);

        return new UserInfoDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            HasPassword = !string.IsNullOrEmpty(user.PasswordHash),
            //Title = user.Title
            IsAuthenticated = principalClaims.Identity!.IsAuthenticated,
            // RoleNames = 
            // IsAdmin = user.Roles.Contains(ApplicationUser.AdminRole),
        };
    }
}