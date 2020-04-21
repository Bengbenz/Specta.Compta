﻿using System;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Infrastructure.Data;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.Server
{
    public static class SeedData
    {
        private static TenantInfo _defaultTenant;

        public static void PopulateAppDatabase(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
            // dbContext.Database.Migrate();

            bool hasInitData = dbContext.ToDoItems.AnyAsync().GetAwaiter().GetResult();
            if (hasInitData)
            {
                return;
            }

            dbContext.ToDoItems.Add(new ToDoItem
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            dbContext.ToDoItems.Add(new ToDoItem
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            dbContext.ToDoItems.Add(new ToDoItem
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });

            dbContext.SaveChanges();
        }

        public static void PopulateDefaultTenantInfo(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<TenantStoreDbContext>();

            dbContext.Database.EnsureCreated();
            // dbContext.Database.Migrate();

            var hasInitData = dbContext.TenantInfo.AnyAsync().GetAwaiter().GetResult();
            if (hasInitData)
            {
                return;
            }

            var configuration = serviceProvider.GetRequiredService<IConfiguration>()
                                               .GetSection("DefaultTenant");
            _defaultTenant = new TenantInfo(configuration.GetValue<string>("Id"),
                                            configuration.GetValue<string>("Identifier"),
                                            configuration.GetValue<string>("Name"),
                                            configuration.GetValue<string>("ConnectionString"),
                                            null);
            dbContext.TenantInfo.Add(_defaultTenant);
            dbContext.SaveChanges();
        } 
    }
}
