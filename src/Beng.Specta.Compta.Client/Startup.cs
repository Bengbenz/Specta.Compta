using System.Net.Http;

using System.Reflection;

using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

using Beng.Specta.Compta.Client.Configs;
using Beng.Specta.Compta.Client.Services;
using Beng.Specta.Compta.Client.ViewModels;
using System;
using Microsoft.AspNetCore.Components;

namespace Beng.Specta.Compta.Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton<WeatherForecastService>();
            services.AddScoped<LayoutInfoVm>();
            services.AddScoped<AppSettings>();
            services.AddScoped<SpinnerService>();
            services.AddScoped<TenantService>();
            services.AddScoped<DisplaySpinnerAutomaticallyHttpMessageHandler>();
            services.AddScoped(s =>
            {
                var blazorDisplaySpinnerAutomaticallyHttpMessageHandler = s.GetRequiredService<DisplaySpinnerAutomaticallyHttpMessageHandler>();
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");

                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
               
                blazorDisplaySpinnerAutomaticallyHttpMessageHandler.InnerHandler = wasmHttpMessageHandler;
                var uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient(blazorDisplaySpinnerAutomaticallyHttpMessageHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            //services.AddLogging(builder => builder.AddBrowserConsole() // Add Blazor.Extensions.Logging.BrowserConsoleLogger
            //                                      .SetMinimumLevel(LogLevel.Trace));

            // Add auth services
            // services.AddAuthorizationCore();
            // services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}

