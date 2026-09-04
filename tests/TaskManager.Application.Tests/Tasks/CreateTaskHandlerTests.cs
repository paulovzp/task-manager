using TaskManager.Application.Abstractions;
using TaskManager.Application.Tasks.Create;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tests.Tasks;

public sealed class CreateTaskHandlerTests
{
    [Fact]
    public async Task HandleAsync_DueDateIsNotFuture_ThrowsArgumentException()
    {
        // Arrange
        var now = new DateTimeOffset(2026, 9, 2, 12, 0, 0, TimeSpan.Zero);
        var handler = new CreateTaskHandler(new RecordingTaskRepository(), new FixedTimeProvider(now));
        var command = new CreateTaskCommand(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Prepare technical presentation",
            "Review architecture decisions and rehearse the demo.",
            now);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.HandleAsync(command, CancellationToken.None));

        // Assert
        Assert.Equal("DueDate", exception.ParamName);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_PersistsPendingTask()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var now = new DateTimeOffset(2026, 9, 2, 12, 0, 0, TimeSpan.Zero);
        var repository = new RecordingTaskRepository();
        var handler = new CreateTaskHandler(repository, new FixedTimeProvider(now));
        var command = new CreateTaskCommand(
            ownerId,
            "Prepare technical presentation",
            "Review architecture decisions and rehearse the demo.",
            now.AddDays(2));

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(repository.AddedTask);
        Assert.Equal(result.Id, repository.AddedTask.Id);
        Assert.Equal(ownerId, result.OwnerId);
        Assert.Equal(TaskItemStatus.Pending, result.Status);
        Assert.True(repository.SavedChanges);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    private sealed class RecordingTaskRepository : ITaskRepository
    {
        public TaskItem? AddedTask { get; private set; }

        public bool SavedChanges { get; private set; }

        public Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken)
        {
            AddedTask = taskItem;
            return Task.CompletedTask;
        }

        public Task<TaskItem?> GetByIdAsync(
            Guid id,
            Guid ownerId,
            CancellationToken cancellationToken) => Task.FromResult<TaskItem?>(null);

        public Task<IReadOnlyList<TaskItem>> ListAsync(
            Guid ownerId,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<TaskItem>>([]);

        public void Remove(TaskItem taskItem)
        {
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            SavedChanges = true;
            return Task.CompletedTask;
        }
    }
}
