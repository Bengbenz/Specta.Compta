using System;

using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Infrastructure.Data;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Server
{
    public static class SeedData
    {
        public static readonly ToDoItem ToDoItem1 = new ToDoItem
        {
            Title = "Get Sample Working",
            Description = "Try to get the sample to build."
        };
        public static readonly ToDoItem ToDoItem2 = new ToDoItem
        {
            Title = "Review Solution",
            Description = "Review the different projects in the solution and how they relate to one another."
        };
        public static readonly ToDoItem ToDoItem3 = new ToDoItem
        {
            Title = "Run and Review Tests",
            Description = "Make sure all the tests run and review what they are doing."
        };

        public static void Initialize(AppDbContext dbContext)
        {
            PopulateTodoItemsData(dbContext);
        }

        public static void InitDefaultTenantInfo(TenantStoreDbContext dbContext)
        {
            foreach (var item in dbContext.TenantInfo)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            // var scopeServices = serviceProvider.CreateScope().ServiceProvider;
            // IMultiTenantStore store = scopeServices.GetRequiredService<IMultiTenantStore>();
            var defaultTenant = new TenantInfo("default-tenant", "defaultTenant", "Default Tenant", "Data Source=BengSpectaCompta.sqlite", null);
            dbContext.TenantInfo.Add(defaultTenant);
            //store.TryAddAsync(defaultTenant).Wait();

            dbContext.SaveChanges();
        }

        private static void PopulateTodoItemsData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.ToDoItems)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.ToDoItems.Add(ToDoItem1);
            dbContext.ToDoItems.Add(ToDoItem2);
            dbContext.ToDoItems.Add(ToDoItem3);

            dbContext.SaveChanges();
        }

        
    }
}
