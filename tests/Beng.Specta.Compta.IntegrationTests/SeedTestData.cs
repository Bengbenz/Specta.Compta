using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.IntegrationTests
{
    public static class SeedTestData
    {
        public static async Task AddProjects(this IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            bool hasInitData = await dbContext.Projects.AnyAsync();
            if (hasInitData)
            {
                return;
            }

            dbContext.Projects.Add(new Project("Test1", "Get Sample Working")
            {
                Description = "Try to get the sample to build."
            });
            dbContext.Projects.Add(new Project("Test2", "Review Solution")
            {
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            dbContext.Projects.Add(new Project("Test3", "Run and Review Tests")
            {
                Description = "Make sure all the tests run and review what they are doing."
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
