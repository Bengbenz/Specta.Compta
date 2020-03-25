using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Beng.Specta.Compta.Core.Entities;

using Finbuckle.MultiTenant.EntityFrameworkCore;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.IsMultiTenant();
            builder.Property(t => t.Title)
                   .IsRequired();
        }
    }
}
