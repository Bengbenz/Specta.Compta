using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Repositories;
using Beng.Specta.Compta.Server;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.UnitTests;
using Beng.Specta.Compta.UnitTests.Helpers;
using Finbuckle.MultiTenant;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests
{
    /// <inheritdoc />
    public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string AppConnectionStringForTest = "Beng.Specta.ComptaStore.Integration.Test";
        private const string TenantConnectionStringForTest = "Beng.Specta.TenantStore.Integration.Test";
        
        public readonly string BaseUrl = $"https://localhost:{GetRandomUnusedPort()}";
        
        public virtual Task InitializeAsync() => Task.CompletedTask;

        public new Task DisposeAsync() => Task.CompletedTask;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.UseUrls(BaseUrl);
            builder.ConfigureServices(services =>
            {
                services.SwapDbContext<AppDbContext>(AppConnectionStringForTest);
                services.SwapDbContext<TenantStoreDbContext>(TenantConnectionStringForTest);

                services.AddScoped(_ => BuildDefaultTenantInfo()); // Add a default Tenant for AppDbContext and repository
                services.AddScoped<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                services.AddScoped<IRepository, GenericRepository>();
                services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            });
        }

        public HttpClient CreateClientWithFakeAuthorization()
        {
            return WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IPolicyEvaluator, ByPassAuthorizationPolicyEvaluator>();
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        public IntegrationTestingWebApplicationFactory AddTestService(Action<IServiceCollection> configureTestServices)
        {
            WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(configureTestServices);
            });

            return this;
        }
        
        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        
        private static ITenantInfo BuildDefaultTenantInfo()
        {
            return new TenantInfo
            {
                Id = "finprod",
                Identifier = "finprod",
                ConnectionString = TenantConnectionStringForTest
            };
        }
    }
}
