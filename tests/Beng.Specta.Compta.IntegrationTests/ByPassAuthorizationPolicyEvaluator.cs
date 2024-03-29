using System.Security.Claims;
using Beng.Specta.Compta.Core.Objects.Identities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Beng.Specta.Compta.IntegrationTests;

public sealed class ByPassAuthorizationPolicyEvaluator : IPolicyEvaluator
{
    public async Task<AuthenticateResult> AuthenticateAsync(
        AuthorizationPolicy policy,
        HttpContext context)
    {
        var testScheme = "FakeScheme";
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("Permission", "CanViewPage"),
            new Claim("Manager", "yes"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.NameIdentifier, "John"),
            new Claim(PermissionConstants.PackedPermissionClaimType, "SuperAdmin")
        }, testScheme);
        var principal = new ClaimsPrincipal(identity);
        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, testScheme)));
    }
        
    public async Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object? resource)
    {
        return await Task.FromResult(PolicyAuthorizationResult.Success());
    }
}