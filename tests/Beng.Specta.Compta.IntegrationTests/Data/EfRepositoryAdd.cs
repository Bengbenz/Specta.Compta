using System.Linq;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.UnitTests;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public class EfRepositoryAdd : BaseEfRepoTestFixture
    {
        //[Fact]
        public void AddsItemAndSetsId()
        {
            var repository = GetRepository();
            var item = new ToDoItemBuilder().Build();

            repository.Add(item);

            var newItem = repository.List<ToDoItem>().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }
    }
}
