using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Beng.Specta.Compta.Infrastructure.DomainEvents;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.Infrastructure.Services;
using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Config
{
    public static class ServiceCollectionExtensions
	{
        private static IConfiguration _configuration;
        
        public static IServiceCollection AddAppDbContext(this IServiceCollection services)
		{
            // App store : Per-tenant Data Store
            services.AddDbContext<AppDbContext>();

			return services;
		}

		public static IServiceCollection AddMultiTenantInfra(this IServiceCollection services)
		{
			// Build the intermediate service provider
			var serviceProvider = services.BuildServiceProvider();
			_configuration = serviceProvider.GetRequiredService<IConfiguration>();
			
			// Allows accessing HttpContext in Blazor
            // services.AddHttpContextAccessor() Already done in AddMultiTenant()
			services.AddMultiTenant<TenantInfo>()
					.WithMultiTenantStore()
					.WithMultiTenantStrategy()
					.WithPerTenantAuthentication();
					
			return services;
		}

		/// <Summary>
		/// Finbuckle.MultiTenant provides four basic multitenant stores :
		/// * InMemoryStore - a simple, thread safe in-memory implementation based on ConcurrentDictionary<string, object>.
		/// * ConfigurationStore - a read-only store that is back by app configuration (e.g. appsettings.json).
		/// * EFCoreStore - an Entity Framework Core based implementation to query tenant information from a database.
		/// ** HttpRemoteStore - a read-only store that sends the tenant identifier to an http(s) endpoint to get the tenant information.
		/// </Summary>
		private static FinbuckleMultiTenantBuilder<TenantInfo>  WithMultiTenantStore(this FinbuckleMultiTenantBuilder<TenantInfo> builder)
		{
            // Register to use the database context and TTenantInfo types show above.
            return builder.WithEFCoreStore<TenantStoreDbContext, TenantInfo>();
		}
		
		private static FinbuckleMultiTenantBuilder<TenantInfo> WithMultiTenantStrategy(this FinbuckleMultiTenantBuilder<TenantInfo> builder)
		{
			// Build the intermediate service provider
			var defaultTenantConfig = _configuration.GetSection("DefaultTenant");
			return builder
						  // A template pattern can be specified with the overloaded version. The default pattern is "__tenant__.*"
						  // For example, a request to "https://contoso.example.com/abc123" would use "contoso" as the identifier when resolving the tenant.
						  .WithHostStrategy()
						  // For example, a request to "https://www.example.com/contoso" would use "contoso" as the identifier when resolving the tenant.
						  .WithBasePathStrategy()
						  // For example, a request to "https://www.example.com/contoso/home/" and a route configuration of {__tenant__}/{controller=Home}/{action=Index} would use "contoso" as the identifier when resolving the tenant.
						  .WithRouteStrategy() 
						  // If the called with e.g. "not_a_tenant.mysite.com" and the identifer "not_a_tenant"
						  // is not in the store, then the identifier of "DefaultTenant" will be used as a fallback.
						  // Make sure to include a multitenant store !
						  .WithStaticStrategy(defaultTenantConfig.GetValue<string>("Identifier"));
						  // .WithRemoteAuthentication() for per-tenant remote authentication
		}

		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
            services.AddScoped<IRepository, EfRepository>()
					.AddScoped<IAuthorizationRepository, AuthorizationRepository>()
					.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

			services.AddTransient<IEmailSender, AuthMessageService>();

			return services;
		}
	}
}
