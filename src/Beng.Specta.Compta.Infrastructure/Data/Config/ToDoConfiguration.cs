using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public class ToDoConfiguration : BaseEntityConfiguration<ToDoItem>
    {
        protected override void Configure()
        {
            Builder.Property(t => t.Title)
                   .IsRequired();
        }
    }
}
