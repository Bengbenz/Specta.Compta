using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Infrastructure.Data;

namespace Beng.Specta.Compta.Server
{
    public sealed class Program
    {
        public static async Task Main(string[] args) => (await BuildWebHostAsync(args)).Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task<IHost> BuildWebHostAsync(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            //a) migrate/create the databases 
            //b) add a SuperUser so you can log in and see what happens
            MigrateDatabases(host);
            await InitAppDatabaseAsync(host);
            return host;
        }

        private static async Task InitAppDatabaseAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                IServiceProvider serviceProvider = scope.ServiceProvider;
                try
                {
                    await serviceProvider.PopulateAppDatabase();
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, $"An error occurred seeding the App DBs. Error: {ex.Message}");
                }
            }
        }

        private static void MigrateDatabases(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            using (var context = services.GetRequiredService<TenantStoreDbContext>())
            {
                context.Database.Migrate();
            }

            using (var context = services.GetRequiredService<AppDbContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}
