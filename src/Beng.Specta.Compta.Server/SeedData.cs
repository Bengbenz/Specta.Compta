using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Repositories;
using Beng.Specta.Compta.Server.Identities.Services;

using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Beng.Specta.Compta.Server
{
    public static class SeedData
    {
        private const string SuperAdminRoleName = "SuperAdmin";

        private static int _seedCounter = -1;
        
        public static async Task PopulateAppDatabaseAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            // Web app starts twice due to the default requests from browser are '/' and '/favicon.png'
            _seedCounter++;
            if (_seedCounter > 0)
            {
                logger.LogWarning($"Database is already seeded. {_seedCounter} times");
                return;
            }
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var tenantInfo = await TryAddDefaultTenantInfoAsync(serviceProvider, configuration);
            await AddSuperAdminAsync(tenantInfo, serviceProvider, configuration);
        }

        private static async Task<ITenantInfo> TryAddDefaultTenantInfoAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            var configSection = configuration.GetSection("DefaultTenant");
            var defaultTenantId = configSection.GetValue<string>("Id");
            
            var scopeServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            var tenantStore = scopeServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
            var logger = serviceProvider.GetRequiredService<ILogger<IMultiTenantStore<TenantInfo>>>();
            TenantInfo? defaultTenant = await tenantStore.TryGetAsync(defaultTenantId);
            if (defaultTenant is not null)
            {
                logger.LogInformation("The default tenant is already exist.");
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
                await tenantStore.TryAddAsync(defaultTenant);
                logger.LogInformation("The default tenant is successfully seeded.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the App DBs:\n{ex.Message}");
            }

            return defaultTenant;
        }

        /// <summary>
        /// This ensures there is a SuperAdmin user in the system.
        /// It gets the SuperAdmin's email and password from the "SuperAdmin" section of the appsettings.json file
        /// NOTE: for security reasons I only allows one user with the RoleName of <see cref="SuperAdminRoleName"/> 
        /// </summary>
        private static async Task AddSuperAdminAsync(
            ITenantInfo tenant,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(tenant);

            var scopeServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            await using var appDbContext = new AppDbContext(tenant, scopeServiceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
            var authorizationRepository = new AuthorizationRepository(appDbContext, scopeServiceProvider.GetRequiredService<ILogger<IAuthorizationRepository>>());
            if (authorizationRepository.IsAnyUsersWithRole(SuperAdminRoleName))
            {
                //For security reasons there can only be one user with the SuperAdminRoleName,
                // So, you delete user, think to delete also the Role (UserToRoles)
                return;
            }

            var superSection = configuration.GetSection("SuperAdmin");
            if (superSection == null)
            {
                return;
            }

            var userEmail = superSection["Email"];
            var userPassword = superSection["Password"];

            var logger = scopeServiceProvider.GetRequiredService<ILogger<UserManager<IdentityUser>>>();
            try
            {
                var userManager = new UserManager<IdentityUser>(
                    new UserOnlyStore<IdentityUser>(appDbContext),
                    null,
                    new PasswordHasher<IdentityUser>(), 
                    new List<IUserValidator<IdentityUser>> { new UserValidator<IdentityUser>()},
                    new List<IPasswordValidator<IdentityUser>> { new PasswordValidator<IdentityUser>()},
                    new UpperInvariantLookupNormalizer(),
                    null,
                    scopeServiceProvider,
                    logger);
                IdentityUser superUser = await userManager.AddNewUserAsync(userEmail, userPassword);
                
                var identityService = new IdentityService(authorizationRepository);
                await identityService.AddOrUpdateRoleToPermissionsAsync(
                    SuperAdminRoleName,
                    "SuperAdmin Role",
                    Permission.AccessAll);

                await identityService.AddRoleToUserAsync(superUser.Id, SuperAdminRoleName);
                logger.LogInformation("The default super admin is successfully seeded.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Seed counter:{_seedCounter}. An error occurred seeding the Super Admin:\n{ex.Message}");
                throw;
            }
        }
    }
}
