using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Auth;

namespace Beng.Specta.Compta.Server.Auth
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// </summary>
    // Thanks to https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
    public class AddPermissionsToUserClaimsFactory : UserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly IAuthorizationRepository _repository;

        public AddPermissionsToUserClaimsFactory(
            UserManager<IdentityUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            IAuthorizationRepository repository)
            : base(userManager, optionsAccessor)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            string userId = identity.Claims.GetUserIdFromClaims();

            var permissionsBuilder = new FeaturePermissionsBuilder(_repository);
            string userComputedPerrmissions = await permissionsBuilder.ComputeUserPermissionsAsync(userId);
            identity.AddClaim(new Claim(PermissionConstants.PackedPermissionClaimType, userComputedPerrmissions));
            return identity;
        }
    }

}