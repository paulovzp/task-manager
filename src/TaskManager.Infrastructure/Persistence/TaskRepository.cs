using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Tasks;

namespace TaskManager.Infrastructure.Persistence;

/// <summary>Persists task items with Entity Framework Core.</summary>
public sealed class TaskRepository(TaskManagerDbContext context) : ITaskRepository
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken) =>
        _context.Tasks.AddAsync(taskItem, cancellationToken).AsTask();

    /// <inheritdoc />
    public Task<TaskItem?> GetByIdAsync(
        Guid id,
        Guid ownerId,
        CancellationToken cancellationToken) =>
        _context.Tasks.SingleOrDefaultAsync(
            task => task.Id == id && task.OwnerId == ownerId,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<TaskItem>> ListAsync(
        Guid ownerId,
        CancellationToken cancellationToken) =>
        await _context.Tasks
            .AsNoTracking()
            .Where(task => task.OwnerId == ownerId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    /// <inheritdoc />
    public void Remove(TaskItem taskItem) => _context.Tasks.Remove(taskItem);

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
