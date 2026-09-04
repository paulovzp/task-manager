using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Abstractions;

/// <summary>
/// Provides persistence operations for user-owned task items.
/// </summary>
public interface ITaskRepository
{
    /// <summary>Adds a task to the persistence unit.</summary>
    Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken);

    /// <summary>Finds a task by identifier and owner.</summary>
    Task<TaskItem?> GetByIdAsync(Guid id, Guid ownerId, CancellationToken cancellationToken);

    /// <summary>Lists all tasks owned by a user.</summary>
    Task<IReadOnlyList<TaskItem>> ListAsync(Guid ownerId, CancellationToken cancellationToken);

    /// <summary>Marks a task for removal.</summary>
    void Remove(TaskItem taskItem);

    /// <summary>Persists the current unit of work.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
