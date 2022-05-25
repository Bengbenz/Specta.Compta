using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.UnitTests.Helpers;

using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data;

public class ProjectRepositoryTest : IClassFixture<RepositoryTestingFactory>
{
    private readonly IRepository _repository;
    private readonly AppDbContext _appDbContext;
    
    public ProjectRepositoryTest(RepositoryTestingFactory factory)
    {
        _repository = factory.GetGenericRepository();
        _appDbContext = factory.AppDbContext!;
    }
        
    [Fact]
    public async Task AddsAndSetsId()
    {
        // Arrange
        var item = new ProjectBuilder()
            .SetCode("Bonjour")
            .SetName("Mrs")
            .Build();

        // Act
        await _repository.AddAsync(item);

        // Assert
        var newItem = (await _repository.ListAsync<Project>()).FirstOrDefault();

        Assert.Equal(item, newItem);
        Assert.True(newItem?.Id > 0);
    }

    [Fact]
    public async Task UpdatesAfterAddingIt()
    {
        // Arrange
        // add an item
        var initialTitle = Guid.NewGuid().ToString();
        var item = new ProjectBuilder().SetCode(initialTitle).SetName("Nothing").Build();
        await _repository.AddAsync(item);

        // detach the item so we get a different instance
        _appDbContext.Entry(item).State = EntityState.Detached;

        // fetch the item and update its title
        var newItem = (await _repository.ListAsync<Project>())
            .FirstOrDefault(i => i.Code == initialTitle);
        Assert.NotNull(newItem);
        Assert.NotSame(item, newItem);
        var newCode = Guid.NewGuid().ToString();
        newItem!.Code = newCode;

        // Act
        // Update the item
        await _repository.UpdateAsync(newItem);
            
        // Assert
        var updatedItem = (await _repository.ListAsync<Project>())
            .FirstOrDefault(i => i.Code == newCode);

        Assert.NotNull(updatedItem);
        Assert.NotEqual(item.Code, updatedItem!.Code);
        Assert.Equal(newItem.Id, updatedItem.Id);
    }

    [Fact]
    public async Task DeletesAfterAddingIt()
    {
        // Arrange
        // add an item
        var initialTitle = Guid.NewGuid().ToString();
        var item = new ProjectBuilder().SetCode(initialTitle).SetName("Bonjour").Build();
        await _repository.AddAsync(item);

        // Act
        // delete the item
        await _repository.DeleteAsync(item);

        // Assert
        var items = await _repository.ListAsync<Project>();
        // verify it's no longer there
        Assert.DoesNotContain(items, i => i.Code == initialTitle);
    }
}