using TaskManager.Application.Tasks.Update;
using TaskManager.Application.Tests.TestDoubles;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tests.Tasks;

public sealed class UpdateTaskHandlerTests
{
    [Fact]
    public async Task HandleAsync_OwnedTask_UpdatesAndPersistsTask()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var now = new DateTimeOffset(2026, 9, 2, 12, 0, 0, TimeSpan.Zero);
        var taskItem = TaskItem.Create(ownerId, "Initial", "Initial", now.AddDays(1));
        var repository = new InMemoryTaskRepository(taskItem);
        var handler = new UpdateTaskHandler(repository, new FixedTimeProvider(now));
        var command = new UpdateTaskCommand(
            taskItem.Id,
            ownerId,
            "Updated",
            "Updated description",
            TaskItemStatus.Completed,
            now.AddDays(3));

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal("Updated", result.Title);
        Assert.Equal(TaskItemStatus.Completed, result.Status);
        Assert.True(repository.SavedChanges);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
