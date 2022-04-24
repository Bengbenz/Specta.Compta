using System;
using System.Net.Http;
using Beng.Specta.Compta.Client.Layout.State;
using Beng.Specta.Compta.Client.Services;
using Beng.Specta.Compta.Client.Services.Auth;
using Beng.Specta.Compta.Client.State;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Syncfusion.Blazor;

namespace Beng.Specta.Compta.Client.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<LayoutState>();
            serviceCollection.AddSingleton<ThemeState>();
            serviceCollection.AddSingleton<TenantService>();
            
            AddWebApiHttpClient(serviceCollection);

            // Register Syncfusion license
            AddSyncfusionBlazorServices(serviceCollection);

            // Add auth services
            AddAuthenticationServices(serviceCollection);

            return serviceCollection;
        }

        private static void AddWebApiHttpClient(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<SpinnerState>();

            // Add custom IHttpClientFactory with custom message handler
            serviceCollection.AddScoped<AutoSpinnerHttpMessageHandler>();
            serviceCollection.AddHttpClient<HttpWebApiClient>("Beng.Specta.WebApi",
                    (sp, client) =>
                    {
                        // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                        var navigationManager = sp.GetRequiredService<NavigationManager>();
                        client.BaseAddress = new Uri(navigationManager.BaseUri);
                    })
                .AddHttpMessageHandler<AutoSpinnerHttpMessageHandler>();

            // Add custom Http client instead of the default
            serviceCollection.AddTransient(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("Beng.Specta.WebApi"));
        }

        private static void AddAuthenticationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorizationCore();
            serviceCollection.AddSingleton<AppAuthenticationStateProvider>();
            serviceCollection.AddSingleton<AuthenticationStateProvider>(s =>
                s.GetRequiredService<AppAuthenticationStateProvider>());
        }

        private static void AddSyncfusionBlazorServices(IServiceCollection serviceCollection)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjI5MTE3QDMxMzgyZTMxMmUzMGNOb3ZpemFZUE9udjJiQXZwWWt4QWdBVWJOTlduYmFiRVVXMGxvUVNFV289");
            serviceCollection.AddSyncfusionBlazor();
        }
    }
}