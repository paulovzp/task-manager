using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Tasks;
using TaskManager.Infrastructure.Identity;

namespace TaskManager.Infrastructure.Persistence.Configurations;

internal sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(task => task.Id);
        builder.Property(task => task.Title).HasMaxLength(200).IsRequired();
        builder.Property(task => task.Description).HasMaxLength(2_000).IsRequired();
        builder.Property(task => task.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(task => task.DueDate).IsRequired();
        builder.HasIndex(task => new { task.OwnerId, task.DueDate });
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(task => task.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
