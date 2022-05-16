using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Beng.Specta.Compta.Core.Objects.Identities;

namespace Beng.Specta.Compta.Server.Identities
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// </summary>
    // Thanks to https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
    public class AddPermissionsToUserClaimsFactory : UserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly FeaturePermissionsBuilder _featurePermissionsBuilder;

        public AddPermissionsToUserClaimsFactory(
            UserManager<IdentityUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            FeaturePermissionsBuilder featurePermissionsBuilder)
            : base(userManager, optionsAccessor)
        {
            _featurePermissionsBuilder = featurePermissionsBuilder ?? throw new ArgumentNullException(nameof(featurePermissionsBuilder));
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            string userId = identity.Claims.GetUserIdFromClaims();

            string userComputedPermissions = await _featurePermissionsBuilder.ComputeUserPermissionsAsync(userId);
            identity.AddClaim(new Claim(PermissionConstants.PackedPermissionClaimType, userComputedPermissions));
            return identity;
        }
    }

}