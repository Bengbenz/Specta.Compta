using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.UnitTests.Helpers;

using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public class ProjectRepositoryTest : BaseRepositoryTestFixture
    {
        [Fact]
        public async Task AddsAndSetsId()
        {
            //SETUP
            var repository = GetEfRepository();
            var item = new Project();

            //ATTEMPT
            await repository.AddAsync(item);

            //VERIFY
            var newItem = (await repository.ListAsync<Project>()).FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }

        [Fact]
        public async Task UpdatesAfterAddingIt()
        {
            //SETUP
            // add an item
            var repository = GetEfRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetCode(initialTitle).Build();

            await repository.AddAsync(item);

            // detach the item so we get a different instance
            AppDbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = (await repository.ListAsync<Project>())
                .FirstOrDefault(i => i.Code == initialTitle);
            Assert.NotNull(newItem);
            Assert.NotSame(item, newItem);
            var newCode = Guid.NewGuid().ToString();
            newItem.Code = newCode;

            //ATTEMPT
            // Update the item
            await repository.UpdateAsync(newItem);
            
            //VERIFY
            var updatedItem = (await repository.ListAsync<Project>())
                .FirstOrDefault(i => i.Code == newCode);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Code, updatedItem.Code);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }

        [Fact]
        public async Task DeletesAfterAddingIt()
        {
            //SETUP
            // add an item
            var repository = GetEfRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetName(initialTitle).Build();
            await repository.AddAsync(item);

            //ATTEMPT
            // delete the item
            await repository.DeleteAsync(item);

            //VERIFY
            var items = await repository.ListAsync<Project>();
            // verify it's no longer there
            Assert.DoesNotContain(items, i => i.Name == initialTitle);
        }
    }
}
