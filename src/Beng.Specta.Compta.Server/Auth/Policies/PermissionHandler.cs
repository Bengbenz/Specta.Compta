using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using Beng.Specta.Compta.Core.Objects.Auth;
using Beng.Specta.Compta.Core.Services.Auth;

namespace Beng.Specta.Compta.Server.Auth.Policies
{
    //thanks to https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (requirement == null) throw new ArgumentNullException(nameof(requirement));

            var permissionsClaim =
                context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
            // If user does not have the scope claim, get out of here
            if (permissionsClaim == null)
            {
                return Task.CompletedTask;
            }

            if (permissionsClaim.Value.IsPermissionAllowed(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}