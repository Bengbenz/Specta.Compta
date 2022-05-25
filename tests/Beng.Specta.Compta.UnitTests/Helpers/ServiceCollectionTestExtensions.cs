using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.UnitTests.Helpers
{
    public static class ServiceCollectionTestExtensions
    {
        public static void SwapDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Remove the app's TDbContext registration.
            var serviceDescriptors = services.Where(
                d => d.ServiceType == typeof(DbContextOptions<TDbContext>) || d.ServiceType == typeof(TDbContext)).ToList();

            foreach (var serviceDescriptor in serviceDescriptors)
            {
                services.Remove(serviceDescriptor);
            }

            // Add a database context (TDbContext) using an in-memory
            // database for testing.
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseInMemoryDatabase(connectionString);
            });
        }

        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Transient"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services"></param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        public static void SwapTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService?> implementationFactory)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(implementationFactory);

            RemoveService<TService>(services, ServiceLifetime.Transient);

            services.AddTransient(typeof(TService), sp => implementationFactory(sp)!);
        }

        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Transient"/> registrations of <see cref="TService"/> and adds in <see cref="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <typeparam name="TImplementation">The test or mock implementation of <see cref="TService"/> to add into <see cref="services"/>.</typeparam>
        /// <param name="services"></param>
        public static void SwapTransient<TService, TImplementation>(
            this IServiceCollection services) 
            where TImplementation : class, TService
        {
            ArgumentNullException.ThrowIfNull(services);

            RemoveService<TService>(services, ServiceLifetime.Transient);

            services.AddTransient(typeof(TService), typeof(TImplementation));
        }

        private static void RemoveService<TService>(IServiceCollection services, ServiceLifetime lifetime)
        {
            var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == lifetime).ToList();
            foreach (var serviceDescriptor in serviceDescriptors)
            {
                services.Remove(serviceDescriptor);
            }
        }
    }
} 