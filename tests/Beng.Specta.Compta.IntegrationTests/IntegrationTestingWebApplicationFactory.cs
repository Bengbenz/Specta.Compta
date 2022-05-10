using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Beng.Specta.Compta.Server;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.UnitTests;
using Beng.Specta.Compta.UnitTests.Helpers;

namespace Beng.Specta.Compta.IntegrationTests
{
    /// <inheritdoc />
    public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public string BaseUrl { get; } = $"https://finprod:{GetRandomUnusedPort()}";
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.UseUrls(BaseUrl);
            builder.ConfigureServices(services =>
            {
                services.SwapDbContext<AppDbContext>("Beng.Specta.ComptaStore.Test");
                services.SwapDbContext<TenantStoreDbContext>("Beng.Specta.TenantStore.Test");

                services.AddScoped<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                services.AddScoped<IRepository, EfRepository>();
                services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
                
                services.BuildServiceProvider();
            });
        }

        public HttpClient CreateClientWithFakeAuthorization()
        {
            return WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                        services.AddSingleton<IPolicyEvaluator, ByPassAuthorizationPolicyEvaluator>());
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
    }
}
