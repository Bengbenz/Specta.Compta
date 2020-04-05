using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data;

namespace Beng.Specta.Compta.Infrastructure
{
    public static class ServiceCollectionExtentions
	{
		public static IServiceCollection AddDbContext(this IServiceCollection services)
		{
			// App store : Per-tenant Data Store
			return services.AddDbContext<AppDbContext>();
		}

		public static IServiceCollection AddMultiTenantInfra(this IServiceCollection services)
		{
			services.AddMultiTenant()
					.WithAppStore()
					.WithAppStrategy();
					
			return services;
		}

		/// <Summary>
		/// Finbuckle.MultiTenant provides four basic multitenant stores :
    	/// * InMemoryStore - a simple, thread safe in-memory implementation based on ConcurrentDictionary<string, object>.
    	/// * ConfigurationStore - a read-only store that is back by app configuration (e.g. appsettings.json).
    	/// * EFCoreStore - an Entity Framework Core based implementation to query tenant information from a database.
    	/// ** HttpRemoteStore - a read-only store that sends the tenant identifier to an http(s) endpoint to get the tenant information.
		/// </Summary>
		private static FinbuckleMultiTenantBuilder WithAppStore(this FinbuckleMultiTenantBuilder builder)
		{
			return builder
						  // Set up a case-insentitive in-memory store.
						  //.WithInMemoryStore()
						  // Register to use the database context and TTenantInfo types show above.
						  .WithEFCoreStore<TenantStoreDbContext>();
		}

		
		private static FinbuckleMultiTenantBuilder WithAppStrategy(this FinbuckleMultiTenantBuilder builder)
		{
			return builder
						  // For example, a request to "https://www.example.com/contoso" would use "contoso" as the identifier when resolving the tenant.
						  //.WithBasePathStrategy()
						  // For example, a request to "https://www.example.com/contoso/home/" and a route configuration of {__tenant__}/{controller=Home}/{action=Index} would use "contoso" as the identifier when resolving the tenant.
						  //.WithRouteStrategy(); 
						  // A template pattern can be specified with the overloaded version. The default pattern is "__tenant__.*"
						  // For example, a request to "https://contoso.example.com/abc123" would use "contoso" as the identifier when resolving the tenant.
						  .WithHostStrategy()
						  // If the called with e.g. "not_a_tenant.mysite.com" and the identifer "not_a_tenant"
						  // is not in the store, then the identifier "defaultTenant" will be used as a fallback.
						  // Make sure to include a multitenant store !
						  .WithFallbackStrategy("finprod");
						  // .WithRemoteAuthentication() for per-tenant remote authentification
		}
	}
}
