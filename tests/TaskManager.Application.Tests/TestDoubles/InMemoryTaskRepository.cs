using TaskManager.Application.Abstractions;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tests.TestDoubles;

internal sealed class InMemoryTaskRepository(params TaskItem[] taskItems) : ITaskRepository
{
    private readonly List<TaskItem> _taskItems = [.. taskItems];

    public bool SavedChanges { get; private set; }

    public Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken)
    {
        _taskItems.Add(taskItem);
        return Task.CompletedTask;
    }

    public Task<TaskItem?> GetByIdAsync(Guid id, Guid ownerId, CancellationToken cancellationToken) =>
        Task.FromResult(_taskItems.SingleOrDefault(task => task.Id == id && task.OwnerId == ownerId));

    public Task<IReadOnlyList<TaskItem>> ListAsync(Guid ownerId, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<TaskItem>>(_taskItems.Where(task => task.OwnerId == ownerId).ToArray());

    public void Remove(TaskItem taskItem) => _taskItems.Remove(taskItem);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        SavedChanges = true;
        return Task.CompletedTask;
    }
}
