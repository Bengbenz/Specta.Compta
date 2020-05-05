using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data.DbContext;
using Beng.Specta.Compta.Server.Security.Auth;
using Beng.Specta.Compta.Server.Security.Auth.Policies;

namespace Beng.Specta.Compta.Server
{
    public static class ContainerExtensions
    {

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
		{
            // Register DbContext for IdentityUser
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppDbContext>();

			//This registers the code to add the Permissions on login
			services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AddPermissionsToUserClaimsFactory>();
		
			//Register the Permission policy handlers
			services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
			services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
					
			return services;
		} 
    }
}