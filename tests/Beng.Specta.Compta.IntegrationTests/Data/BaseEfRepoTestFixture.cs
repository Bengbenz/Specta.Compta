﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data.DbContext;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public abstract class BaseEfRepoTestFixture
    {
        protected AppDbContext AppDbContext { get; private set; }

        // protected AppDbContext TennantDbContext { get; private set; }

        protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("Beng.Specta.Compta.IntegrationTest")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected EfRepository GetRepository()
        {
            // var mockDispatcher = new Mock<IDomainEventDispatcher>();

            //_dbContext = new AppDbContext(options, mockDispatcher.Object);
            var defaultTenant = new TenantInfo("defaultTenant", "defaultTenant", "Default Tenant", "", null);
            AppDbContext = new AppDbContext(defaultTenant, CreateNewContextOptions());
            return new EfRepository(AppDbContext);
        }
    }
}
