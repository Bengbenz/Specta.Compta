using System.Threading.Tasks;

using Beng.Specta.Compta.Client.Config;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Beng.Specta.Compta.Client
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args); 
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Add services to the container
            builder.Services
                .AddSyncfusionBlazorServices()
                .AddClientServices();

            await builder.Build().RunAsync();
        }
    }
}
