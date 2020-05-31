using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.Objects.Auth;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Server.Auth.Services
{
    public static class SuperAdminExtension
    {
        private const string SuperAdminRoleName = "SuperAdmin";

        /// <summary>
        /// This ensures there is a SuperAdmin user in the system.
        /// It gets the SuperAdmin's email and password from the "SuperAdmin" section of the appsettings.json file
        /// NOTE: for security reasons I only allows one user with the RoleName of <see cref="SuperAdminRoleName"/> 
        /// </summary>
        public static async Task AddSuperAdminAsync(this IServiceProvider serviceProvider, TenantInfo tenant)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                var config = serviceProvider.GetRequiredService<IConfiguration>();
                using (var appContext = new AppDbContext(tenant, config))
                {
                    if (appContext.UserToRoles.Any(x => x.RoleName == SuperAdminRoleName))
                    {
                        //For security reasons there can only be one user with the SuperAdminRoleName
                        return;
                    }

                    var superSection = config.GetSection("SuperAdmin");
                    if (superSection == null)
                    {
                        return;
                    }

                    var userEmail = superSection["Email"];
                    var userPassword = superSection["Password"];

                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    IdentityUser superUser = await userManager.AddNewUserAsync(userEmail, userPassword);

                    var repoLogger = serviceProvider.GetRequiredService<ILogger<AuthorizationRepository>>();
                    var authService = new AuthUsersService(new AuthorizationRepository(appContext, repoLogger));
                    await authService.AddOrUpdateRoleToPermissionsAsync(
                        SuperAdminRoleName,
                        "SuperAdmin Role",
                        Permission.AccessAll);

                    await authService.AddRoleToUserAsync(superUser.Id, SuperAdminRoleName);
                }
            }
        }
    }
}