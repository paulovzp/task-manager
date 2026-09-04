using TaskManager.Application.Tasks.Delete;
using TaskManager.Application.Tests.TestDoubles;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tests.Tasks;

public sealed class DeleteTaskHandlerTests
{
    [Fact]
    public async Task HandleAsync_OwnedTask_RemovesAndPersistsTask()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var taskItem = TaskItem.Create(ownerId, "Delete me", "No longer needed", DateTimeOffset.UtcNow.AddDays(1));
        var repository = new InMemoryTaskRepository(taskItem);
        var handler = new DeleteTaskHandler(repository);

        // Act
        await handler.HandleAsync(new DeleteTaskCommand(taskItem.Id, ownerId), CancellationToken.None);

        // Assert
        Assert.Null(await repository.GetByIdAsync(taskItem.Id, ownerId, CancellationToken.None));
        Assert.True(repository.SavedChanges);
    }
}
