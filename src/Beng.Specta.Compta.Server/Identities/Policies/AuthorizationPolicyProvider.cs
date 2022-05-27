// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Beng.Specta.Compta.Server.Identities.Policies;
//thanks to https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/
//And to GholamReza Rabbal see https://github.com/JonPSmith/PermissionAccessControl/issues/3

public sealed class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        ArgumentNullException.ThrowIfNull(policyName);

        //Unit tested shows this is quicker (and safer - see link to issue above) than the original version
        return await base.GetPolicyAsync(policyName)
               ?? new AuthorizationPolicyBuilder()
                   .AddRequirements(new PermissionRequirement(policyName))
                   .Build();
    }
}