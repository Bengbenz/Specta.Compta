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
            // Arrange
            var repository = GetEfRepository();
            var item = new ProjectBuilder()
                .SetCode("Bonjour")
                .SetName("Mrs")
                .Build();

            // Act
            await repository.AddAsync(item);

            // Assert
            var newItem = (await repository.ListAsync<Project>()).FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }

        [Fact]
        public async Task UpdatesAfterAddingIt()
        {
            // Arrange
            // add an item
            var repository = GetEfRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetCode(initialTitle).SetName("Nothing").Build();

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

            // Act
            // Update the item
            await repository.UpdateAsync(newItem);
            
            // Assert
            var updatedItem = (await repository.ListAsync<Project>())
                .FirstOrDefault(i => i.Code == newCode);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Code, updatedItem.Code);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }

        [Fact]
        public async Task DeletesAfterAddingIt()
        {
            // Arrange
            // add an item
            var repository = GetEfRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProjectBuilder().SetCode(initialTitle).SetName("Bonjour").Build();
            await repository.AddAsync(item);

            // Act
            // delete the item
            await repository.DeleteAsync(item);

            // Assert
            var items = await repository.ListAsync<Project>();
            // verify it's no longer there
            Assert.DoesNotContain(items, i => i.Code == initialTitle);
        }
    }
}
