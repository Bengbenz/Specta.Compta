using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.UnitTests.Helpers;

using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public class ProjectRepositoryTest : BaseEfRepoTestFixture
    {
        [Fact]
        public void AddsAndSetsId()
        {
            var repository = GetRepository();
            var item = new Project();

            repository.Add(item);

            var newItem = repository.List<Project>().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }

        [Fact]
        public void UpdatesAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetCode(initialTitle).Build();

            repository.Add(item);

            // detach the item so we get a different instance
            AppDbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = repository.List<Project>()
                .FirstOrDefault(i => i.Code == initialTitle);
            Assert.NotNull(newItem);
            Assert.NotSame(item, newItem);
            var newCode = Guid.NewGuid().ToString();
            newItem.Code = newCode;

            // Update the item
            repository.Update(newItem);
            var updatedItem = repository.List<Project>()
                .FirstOrDefault(i => i.Code == newCode);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Code, updatedItem.Code);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }

        [Fact]
        public async Task DeletesAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetName(initialTitle).Build();
            repository.Add(item);

            // delete the item
            await repository.DeleteAsync(item);

            var items = repository.List<Project>();
            // verify it's no longer there
            Assert.DoesNotContain(items, i => i.Name == initialTitle);
        }
    }
}
