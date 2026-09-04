using TaskManager.Application.Common;
using TaskManager.Application.Tasks.Get;
using TaskManager.Application.Tests.TestDoubles;

namespace TaskManager.Application.Tests.Tasks;

public sealed class GetTaskHandlerTests
{
    [Fact]
    public async Task HandleAsync_TaskIsNotOwnedByUser_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new GetTaskHandler(new InMemoryTaskRepository());
        var query = new GetTaskQuery(
            Guid.NewGuid(),
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"));

        // Act
        var exception = await Assert.ThrowsAsync<TaskNotFoundException>(() =>
            handler.HandleAsync(query, CancellationToken.None));

        // Assert
        Assert.Equal(query.TaskId, exception.TaskId);
    }
}
