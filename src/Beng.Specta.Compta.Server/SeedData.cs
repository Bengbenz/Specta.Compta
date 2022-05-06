using System;
using System.Threading.Tasks;
using Beng.Specta.Compta.Core.Objects.Auth;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Server.Auth.Services;

using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;

namespace Beng.Specta.Compta.Server
{
    public static class SeedData
    {
        private const string SuperAdminRoleName = "SuperAdmin";
        
        public static async Task PopulateAppDatabaseAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var defaultTenant = await TryAddDefaultTenantInfoAsync(serviceProvider, configuration, logger);
            // await AddSuperAdminAsync(scopeServices, configuration, defaultTenant);
        }

        private static async Task<TenantInfo> TryAddDefaultTenantInfoAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger logger)
        {
            
            var configSection = configuration.GetSection("DefaultTenant");
            var defaultTenantId = configSection.GetValue<string>("Id");
            
            var scopeServices = serviceProvider.CreateScope().ServiceProvider;
            var store = scopeServices.GetRequiredService<IMultiTenantStore<TenantInfo>>();
            var defaultTenant = await store.TryGetAsync(defaultTenantId);
            if (defaultTenant is not null)
            {
                return defaultTenant;
            }
            
            defaultTenant = new TenantInfo
            {
                Id = defaultTenantId,
                Identifier = configSection.GetValue<string>("Identifier"),
                Name = configSection.GetValue<string>("Name"),
                ConnectionString = configSection.GetValue<string>("ConnectionString")
            };
            
            try
            {
                await store.TryAddAsync(defaultTenant);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the App DBs. Error: {ex.Message}");
            }

            return defaultTenant;
        }

        /// <summary>
        /// This ensures there is a SuperAdmin user in the system.
        /// It gets the SuperAdmin's email and password from the "SuperAdmin" section of the appsettings.json file
        /// NOTE: for security reasons I only allows one user with the RoleName of <see cref="SuperAdminRoleName"/> 
        /// </summary>
        private static async Task AddSuperAdminAsync(IServiceProvider serviceProvider,
            IConfiguration configuration,
            TenantInfo tenant)
        {
            if (tenant is null) throw new ArgumentNullException(nameof(tenant));

            await using var appContext = new AppDbContext(tenant);
            var repoLogger = serviceProvider.GetRequiredService<ILogger<AuthorizationRepository>>();
            var authorizationRepository = new AuthorizationRepository(appContext, repoLogger);
            if (authorizationRepository.IsAnyUsersWithRole(SuperAdminRoleName))
            {
                //For security reasons there can only be one user with the SuperAdminRoleName
                return;
            }

            var superSection = configuration.GetSection("SuperAdmin");
            if (superSection == null)
            {
                return;
            }

            var userEmail = superSection["Email"];
            var userPassword = superSection["Password"];

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser superUser = await userManager.AddNewUserAsync(userEmail, userPassword);
            
            var authService = new AuthUsersService(authorizationRepository);
            await authService.AddOrUpdateRoleToPermissionsAsync(
                SuperAdminRoleName,
                "SuperAdmin Role",
                Permission.AccessAll);

            await authService.AddRoleToUserAsync(superUser.Id, SuperAdminRoleName);
        }
    }
}
