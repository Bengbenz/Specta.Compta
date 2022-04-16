﻿using System;
using System.Net.Http;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Beng.Specta.Compta.Server;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.UnitTests;
using Beng.Specta.Compta.UnitTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;

namespace Beng.Specta.Compta.FunctionalTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices(services =>
            {
                services.SwapDbContext<AppDbContext>("Beng.Specta.ComptaStore.Test");
                services.SwapDbContext<TenantStoreDbContext>("Beng.Specta.TenantStore.Test");

                services.AddScoped<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                services.AddScoped<IRepository, EfRepository>();
                services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

                // services.AddCustomAuthorization();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (AppDbContext).
                using (var scope = sp.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        // Seed the database with test data.
                        serviceProvider.PopulateAppDatabase().GetAwaiter();
                    }
                    catch (Exception ex)
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                        logger.LogError(ex, "An error occurred seeding the " +
                            $"database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }

        public HttpClient CreateClientWithoutAuthorization()
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

        public WebApplicationFactory<TStartup> AddTestService(Action<IServiceCollection> configureTestServices)
        {
            WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(configureTestServices);
            });

            return this;
        }
    }
}
