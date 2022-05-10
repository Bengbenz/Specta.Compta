using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            dbContext.Projects.Add(new Project
            {
                Name = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            dbContext.Projects.Add(new Project
            {
                Name = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            dbContext.Projects.Add(new Project
            {
                Name = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
