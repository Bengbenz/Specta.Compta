using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Server.Auth.Services;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Server
{
    public static class SeedData
    {
        private static IConfiguration _configuration;

        public static async Task PopulateAppDatabase(this IServiceProvider serviceProvider)
        {
            var defaultTenant = await serviceProvider.PopulateDefaultTenantInfo();

            // await serviceProvider.AddSuperAdminAsync(defaultTenant);
        }

        private static async Task<TenantInfo> PopulateDefaultTenantInfo(this IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<TenantStoreDbContext>();

            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var configSection = _configuration.GetSection("DefaultTenant");

            var defaultTenantId = configSection.GetValue<string>("Id");
            TenantInfo defaultTenant = await dbContext.TenantInfo.SingleOrDefaultAsync(x => x.Id == defaultTenantId);

            if (defaultTenant == null)
            {
                defaultTenant = new TenantInfo(configSection.GetValue<string>("Id"),
                                    configSection.GetValue<string>("Identifier"),
                                    configSection.GetValue<string>("Name"),
                                    configSection.GetValue<string>("ConnectionString"),
                                    new Dictionary<string, object>());
                dbContext.TenantInfo.Add(defaultTenant);
                dbContext.SaveChanges();
            }

            return defaultTenant;
        }
    }
}
