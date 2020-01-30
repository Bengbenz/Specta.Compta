using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Beng.Specta.Compta.Core;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Beng.Specta.Compta.Infrastructure
{
    public static class ContainerSetup
    {
        public static IServiceProvider InitializeWeb(Assembly webAssembly, IServiceCollection services) =>
            new AutofacServiceProvider(BaseAutofacInitialization(setupAction =>
            {
                setupAction.Populate(services);
                setupAction.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces();
            }));

        public static IContainer BaseAutofacInitialization(Action<ContainerBuilder> setupAction = null)
        {
            var builder = new ContainerBuilder();

            var coreAssembly = Assembly.GetAssembly(typeof(DatabasePopulator));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            var sharedKernelAssembly = Assembly.GetAssembly(typeof(IRepository));
            builder.RegisterAssemblyTypes(sharedKernelAssembly, coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

            setupAction?.Invoke(builder);
            return builder.Build();
        }
    }
}
