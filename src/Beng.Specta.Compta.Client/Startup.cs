using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Client.Configs;
using Beng.Specta.Compta.Client.ViewModels;

using Blazor.Extensions.Logging;

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

            //services.AddLogging(builder => builder.AddBrowserConsole() // Add Blazor.Extensions.Logging.BrowserConsoleLogger
            //                                      .SetMinimumLevel(LogLevel.Trace));

            // Add auth services
            // services.AddAuthorizationCore();
            // services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            // add scss service
            //services.AddWebOptimizer(pipeline =>
            // {
            //    pipeline.AddScssBundle("/css/site.css", "/scss/app.scss");
            // });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}

