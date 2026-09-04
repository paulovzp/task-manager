using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Tasks;
using TaskManager.Infrastructure.Identity;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Tests.Persistence;

public sealed class TaskRepositoryTests
{
    [Fact]
    public async Task ListAsync_TasksHaveDifferentOwners_ReturnsOnlyOwnedTasks()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new TaskManagerDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var anotherOwnerId = Guid.NewGuid();
        context.Users.AddRange(
            new ApplicationUser { Id = ownerId, Name = "Owner", Email = "owner@example.com", UserName = "owner@example.com" },
            new ApplicationUser { Id = anotherOwnerId, Name = "Other", Email = "other@example.com", UserName = "other@example.com" });
        await context.SaveChangesAsync();
        var repository = new TaskRepository(context);
        await repository.AddAsync(
            TaskItem.Create(ownerId, "Owned", "Visible", DateTimeOffset.UtcNow.AddDays(1)),
            CancellationToken.None);
        await repository.AddAsync(
            TaskItem.Create(anotherOwnerId, "Other", "Hidden", DateTimeOffset.UtcNow.AddDays(1)),
            CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.ListAsync(ownerId, CancellationToken.None);

        // Assert
        var task = Assert.Single(result);
        Assert.Equal("Owned", task.Title);
    }
}
