﻿using System;
using System.Net.Http;
using System.Reflection;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Client.Layout.State;
using Beng.Specta.Compta.Client.Services;

using Beng.Specta.Compta.Client.Services.Auth;

using Beng.Specta.Compta.Client.State;

using Syncfusion.Blazor;


namespace Beng.Specta.Compta.Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpApiClientRequestBuilder, HttpApiClientRequestBuilder>();

            services.AddSingleton<LayoutState>();
            services.AddSingleton<ThemeState>();
            services.AddSingleton<TenantService>();
            services.AddSingleton<SpinnerState>();
            
            services.AddScoped<AutoSpinnerHttpMessageHandler>();
            services.AddScoped(s =>
            {
                var spinnerHttpMessageHandler = s.GetRequiredService<AutoSpinnerHttpMessageHandler>();
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");

                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
               
                spinnerHttpMessageHandler.InnerHandler = wasmHttpMessageHandler;
                var uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient(spinnerHttpMessageHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });
            
            // Register Syncfusion license 
            // versionn 17: MjI5MDM0QDMxMzcyZTM0MmUzME9EeXhJcGFzV1FmbzhWcXI4eWE3UGV4RDRiNTB4R0l5b2QvZE4ybmM1Tmc9
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjI5MTE3QDMxMzgyZTMxMmUzMGNOb3ZpemFZUE9udjJiQXZwWWt4QWdBVWJOTlduYmFiRVVXMGxvUVNFV289");
            services.AddSyncfusionBlazor(); 

            // Add auth services
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddSingleton<AppAuthenticationStateProvider>();
            services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<AppAuthenticationStateProvider>());
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}

