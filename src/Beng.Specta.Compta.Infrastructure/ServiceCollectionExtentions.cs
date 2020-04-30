using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.Infrastructure
{
    public static class ServiceCollectionExtentions
	{
        private static IConfiguration _configuration;

        public static IServiceCollection AddDbContext(this IServiceCollection services)
		{
			// Build the intermediate service provider
    		var serviceProvider = services.BuildServiceProvider();
			_configuration = serviceProvider.GetRequiredService<IConfiguration>();

			//Tenants Store
            services.AddDbContext<TenantStoreDbContext>(options =>                    
    					options.UseNpgsql(_configuration.GetConnectionString("TenantConnection"))); // will be created in server project root

            // App store : Per-tenant Data Store
            services.AddDbContext<AppDbContext>(options =>
						options.UseNpgsql(_configuration.GetConnectionString("AppConnection")));

			return services;
		}

		public static IServiceCollection AddMultiTenantInfra(this IServiceCollection services)
		{
			//Allows accessing HttpContext in Blazor
            //services.AddHttpContextAccessor() Already done in AddMultiTenant()
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
            // Set up a case-insentitive in-memory store.
            //.WithInMemoryStore()
            // Register to use the database context and TTenantInfo types show above.
            return builder.WithEFCoreStore<TenantStoreDbContext>();
		}

		
		private static FinbuckleMultiTenantBuilder WithAppStrategy(this FinbuckleMultiTenantBuilder builder)
		{
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
						  // is not in the store, then the identifier "defaultTenant" will be used as a fallback.
						  // Make sure to include a multitenant store !
						  .WithFallbackStrategy(defaultTenantConfig.GetValue<string>("Identifier"));
						  // .WithRemoteAuthentication() for per-tenant remote authentification
		}

		public static IServiceProvider AddAppWebServices(this IServiceCollection services, Assembly webAssembly) =>
			new AutofacServiceProvider(BaseAutofacInitialization(setupAction =>
			{
				setupAction.Populate(services);
				setupAction.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces();
			}));

        public static IContainer BaseAutofacInitialization(Action<ContainerBuilder> setupAction = null)
		{
			var container = new ContainerBuilder();

			var coreAssembly = typeof(BaseDTO).Assembly;
			var infrastructureAssembly = typeof(EfRepository).Assembly;
			var sharedKernelAssembly = typeof(IRepository).Assembly;
			container.RegisterAssemblyTypes(sharedKernelAssembly, coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

			setupAction?.Invoke(container);
			return container.Build();
		}
	}
}
