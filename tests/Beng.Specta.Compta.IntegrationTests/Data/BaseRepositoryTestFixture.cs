using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Finbuckle.MultiTenant;
using Moq;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public abstract class BaseRepositoryTestFixture
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
            builder.UseInMemoryDatabase("Beng.Specta.ComptaStore.Test")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected EfRepository GetEfRepository()
        {
            var mockDispatcher = new Mock<IDomainEventDispatcher>();
            
            AppDbContext = new AppDbContext(BuildDefaultTenantInfo(), CreateNewContextOptions(), mockDispatcher.Object);
            return new EfRepository(AppDbContext);
        }

        private TenantInfo BuildDefaultTenantInfo()
        {
            return new TenantInfo
            {
                Id = "finprod",
                Identifier = "finprod",
                ConnectionString = "Default_Connection_String",
            };
        }

        protected AuthorizationRepository GetAuthorizationRepository()
        {
            var mockDispatcher = new Mock<IDomainEventDispatcher>();
            
            AppDbContext = new AppDbContext(BuildDefaultTenantInfo(), CreateNewContextOptions(), mockDispatcher.Object);
            return new AuthorizationRepository(AppDbContext);
        }
    }
}
