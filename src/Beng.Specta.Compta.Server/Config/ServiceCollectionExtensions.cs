using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Server.Auth;
using Beng.Specta.Compta.Server.Auth.Policies;

namespace Beng.Specta.Compta.Server.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
		{
            // Register DbContext for IdentityUser
            services.AddDefaultIdentity<IdentityUser>(options => 
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.User.RequireUniqueEmail = true;
				})
                .AddEntityFrameworkStores<AppDbContext>();

			// This registers the code to add the Permissions on login
			services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AddPermissionsToUserClaimsFactory>();
		
			// Register the Permission policy handlers
			services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
			services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
					
			return services;
		} 
    }
}