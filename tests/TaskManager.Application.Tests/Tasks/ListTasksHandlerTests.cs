using TaskManager.Application.Tasks.List;
using TaskManager.Application.Tests.TestDoubles;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tests.Tasks;

public sealed class ListTasksHandlerTests
{
    [Fact]
    public async Task HandleAsync_UserHasTasks_ReturnsOnlyOwnedTasks()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var anotherOwnerId = Guid.Parse("1e3c39cd-4184-4e6f-abee-cc753612bed4");
        var ownedTask = TaskItem.Create(ownerId, "Owned", "Visible", DateTimeOffset.UtcNow.AddDays(1));
        var otherTask = TaskItem.Create(anotherOwnerId, "Other", "Hidden", DateTimeOffset.UtcNow.AddDays(1));
        var handler = new ListTasksHandler(new InMemoryTaskRepository(ownedTask, otherTask));

        // Act
        var result = await handler.HandleAsync(new ListTasksQuery(ownerId), CancellationToken.None);

        // Assert
        var task = Assert.Single(result);
        Assert.Equal(ownedTask.Id, task.Id);
    }
}
