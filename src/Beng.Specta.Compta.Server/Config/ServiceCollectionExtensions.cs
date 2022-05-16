using System.Net.Mime;
using Beng.Specta.Compta.Core.Config;
using Beng.Specta.Compta.Infrastructure.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Server.Identities;
using Beng.Specta.Compta.Server.Identities.Policies;
using Beng.Specta.Compta.Server.Identities.Services;
using Microsoft.AspNetCore.ResponseCompression;

namespace Beng.Specta.Compta.Server.Config
{
    public static class ServiceCollectionExtensions
    {
	    ///<summary>
	    /// Use this method to add services to the container.
	    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
	    ///</summary>
	    public static void ConfigureServices(this IServiceCollection services)
	    {
		    services.AddMvc()
			    .AddNewtonsoftJson();

		    services.AddResponseCompression(options =>
		    {
			    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
				    new[] { MediaTypeNames.Application.Octet });
		    });
            
		    services.AddAppDbContext()
			    .AddDatabaseDeveloperPageExceptionFilter()
			    .AddAppIdentityServices()
			    .AddMultiTenantInfra()
			    .AddInfrastructureServices()
			    .AddAppCoreServices();
	    }

	    private static IServiceCollection AddAppIdentityServices(this IServiceCollection services)
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
			services.AddTransient<IdentityService>();
			services.AddTransient<FeaturePermissionsBuilder>();
					
			return services;
		} 
    }
}