using Microsoft.AspNetCore.Authorization;

using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.Core.Services.Identities;

namespace Beng.Specta.Compta.Server.Identities.Policies;
//thanks to https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(requirement);

        var permissionsClaim =
            context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
        // If user does not have the scope claim, get out of here
        if (permissionsClaim is null)
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