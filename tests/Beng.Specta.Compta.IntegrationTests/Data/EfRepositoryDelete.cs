﻿using System;
using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.UnitTests;
using Xunit;

namespace Beng.Specta.Compta.IntegrationTests.Data
{
    public class EfRepositoryDelete : BaseEfRepoTestFixture
    {
        [Fact(Skip = "To fix after")]
        public void DeletesItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ToDoItemBuilder().Title(initialTitle).Build();
            repository.Add(item);

            // delete the item
            repository.Delete(item);

            // verify it's no longer there
            Assert.DoesNotContain(repository.List<ToDoItem>(),
                i => i.Title == initialTitle);
        }
    }
}
