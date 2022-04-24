using Beng.Specta.Compta.Core.Events;
using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.Core.Handlers;
using Beng.Specta.Compta.Core.Handlers.Funding;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.Core.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppCoreServices(this IServiceCollection services)
        {
            services.AddEventHandlers();

            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<IHandle<ToDoItemCompletedEvent>, ItemCompletedEmailNotificationHandler>()
                .AddScoped<IHandle<ContractStepCompletedEvent>, ContractStepCompletedNotificationHandler>();

            return services;
        }
    }
}
