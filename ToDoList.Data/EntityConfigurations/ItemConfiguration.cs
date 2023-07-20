using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;

namespace ToDoList.Data.EntityConfigurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(i => i.Title).HasColumnName("Title").HasMaxLength(100);
            builder.Property(i => i.Description).HasColumnName("Description").HasMaxLength(1000);
            builder.Property(i => i.Status).HasColumnName("Status").ValueGeneratedNever();

            builder.HasOne(i => i.Priority)
                   .WithMany()
                   .HasForeignKey(i => i.PriorityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
