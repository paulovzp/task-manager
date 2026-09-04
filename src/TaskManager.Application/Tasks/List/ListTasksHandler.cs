using TaskManager.Application.Abstractions;

namespace TaskManager.Application.Tasks.List;

/// <summary>Lists task items for one owner.</summary>
public sealed class ListTasksHandler(ITaskRepository repository)
{
    private readonly ITaskRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>Handles a list-tasks request.</summary>
    public async Task<IReadOnlyList<TaskItemDto>> HandleAsync(
        ListTasksQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var tasks = await _repository.ListAsync(query.OwnerId, cancellationToken).ConfigureAwait(false);
        return tasks.Select(TaskItemDto.FromDomain).ToArray();
    }
}
