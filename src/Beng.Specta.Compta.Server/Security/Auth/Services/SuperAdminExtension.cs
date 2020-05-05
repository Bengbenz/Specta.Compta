// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Core.Entities.Security.Permissions;
using Beng.Specta.Compta.Infrastructure.Data.DbContext;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;

namespace Beng.Specta.Compta.Server.Security.Auth.Services
{
    public static class SuperAdminExtension
    {
        private const string SuperAdminRoleName = "SuperAdmin";

        /// <summary>
        /// This ensures there is a SuperAdmin user in the system.
        /// It gets the SuperAdmin's email and password from the "SuperAdmin" section of the appsettings.json file
        /// NOTE: for security reasons I only allows one user with the RoleName of <see cref="SuperAdminRoleName"/> 
        /// </summary>
        public static async Task CheckAddSuperAdminAsync(this IServiceProvider serviceProvider, AppDbContext dbContext)
        {
            var repository = new AuthorizationRepository(dbContext);
            var roleToPermissions = await repository.GetRoleToPermissionAsync(SuperAdminRoleName);

            if (roleToPermissions != null)
            {
                //For security reasons there can only be one user with the SuperAdminRoleName
                return;
            }
                
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var superSection = config.GetSection("SuperAdmin");
            if (superSection == null)
            {
                return;
            }

            var userEmail = superSection["Email"];
            var userPassword = superSection["Password"];

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser superUser = await userManager.CheckAddNewUserAsync(userEmail, userPassword);

            var authService = new AuthUsersService(repository);
            await authService.AddUpdateRoleToPermissionsAsync(
                SuperAdminRoleName,
                "SuperAdmin Role",
                Permission.AccessAll);

            await authService.CheckAddRoleToUserAsync(superUser.Id, SuperAdminRoleName);
        }
    }
}