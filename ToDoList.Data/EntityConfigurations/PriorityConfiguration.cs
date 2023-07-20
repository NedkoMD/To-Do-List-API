using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;

namespace ToDoList.Data.EntityConfigurations
{
    public class PriorityConfiguration : IEntityTypeConfiguration<Priority>
    {
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
            builder.ToTable("Priority");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id");
            builder.Property(p => p.Title).HasColumnName("Title").HasMaxLength(20);
            builder.HasData(
                    new Priority { Id = 1, Title = "Low" },
                    new Priority { Id = 2, Title = "Medium" },
                    new Priority { Id = 3, Title = "High" }
                );
        }
    }
}
