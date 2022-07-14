using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.IntegrationTests
{
    public static class SeedTestData
    {
        public static async Task AddThreeProjects(this IServiceProvider serviceProvider)
        {
            var scopeServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            var repository = scopeServiceProvider.GetRequiredService<IRepository>();
            bool hasInitData = await repository.AnyAsync<Project>();
            if (hasInitData)
            {
                return;
            }
            await repository.AddAsync(new Project("Test1", "Get Sample Working")
            {
                Description = "Try to get the sample to build."
            },
            new Project("Test2", "Review Solution")
            {
                Description = "Review the different projects in the solution and how they relate to one another."
            },
            new Project("Test3", "Run and Review Tests")
            {
                Description = "Make sure all the tests run and review what they are doing."
            });
        }
        
        public static async Task CleanProjects(this IServiceProvider serviceProvider)
        {
            var scopeServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            var repository = scopeServiceProvider.GetRequiredService<IRepository>();
            var projects = await repository.ListAsync<Project>();
            if (projects.Any())
            {
                await repository.DeleteAsync(projects.ToArray());
            }
        }
    }
}
