using Beng.Specta.Compta.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Repositories;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Finbuckle.MultiTenant;
using Moq;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data;

public class RepositoryTestingFactory : IAsyncLifetime
{
    private const string AppConnectionStringForTest = "Beng.Specta.ComptaStore.Repository.Test";
    private const string TenantConnectionStringForTest = "Beng.Specta.TenantStore.Repository.Test";
    public AppDbContext? AppDbContext { get; private set; }

    // protected AppDbContext TenantDbContext { get; private set; }

    private static DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(AppConnectionStringForTest)
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    public IRepository GetGenericRepository()
    {
        var mockLogger = new Mock<ILogger<IRepository>>();
        
        return new GenericRepository(AppDbContext!, mockLogger.Object);
    }

    private TenantInfo BuildDefaultTenantInfo()
    {
        return new TenantInfo
        {
            Id = "finprod",
            Identifier = "finprod",
            ConnectionString = TenantConnectionStringForTest
        };
    }

    protected AuthorizationRepository GetAuthorizationRepository()
    {
        var mockLogger = new Mock<ILogger<IAuthorizationRepository>>();
            
        return new AuthorizationRepository(AppDbContext!, mockLogger.Object);
    }

    public Task InitializeAsync()
    {
        var mockDispatcher = new Mock<IDomainEventDispatcher>();
        AppDbContext = new AppDbContext(BuildDefaultTenantInfo(), CreateNewContextOptions(), mockDispatcher.Object);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await AppDbContext!.DisposeAsync();
    }
}