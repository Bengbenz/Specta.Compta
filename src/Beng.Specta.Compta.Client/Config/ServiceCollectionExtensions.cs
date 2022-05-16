using System;
using System.Net.Http;
using Beng.Specta.Compta.Client.Layout.State;
using Beng.Specta.Compta.Client.Services;
using Beng.Specta.Compta.Client.Services.Identities;
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

        public static IServiceCollection AddSyncfusionBlazorServices(this IServiceCollection serviceCollection)
        {
            // Register Syncfusion license
            var licenseKey = "NjI1MzY4QDMyMzAyZTMxMmUzMFY3Sm9ieE1KU3hYcFk0bmE4S0FzWDNBeTZpblg1VGhBM2hldUlQYXJXRjg9";
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            serviceCollection.AddSyncfusionBlazor();

            return serviceCollection;
        }
    }
}