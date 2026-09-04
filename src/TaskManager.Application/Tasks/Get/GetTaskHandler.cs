using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.Tasks.Get;

/// <summary>Retrieves one user-owned task.</summary>
public sealed class GetTaskHandler(ITaskRepository repository)
{
    private readonly ITaskRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>Handles a get-task request.</summary>
    public async Task<TaskItemDto> HandleAsync(
        GetTaskQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var taskItem = await _repository
            .GetByIdAsync(query.TaskId, query.OwnerId, cancellationToken)
            .ConfigureAwait(false);

        return taskItem is null
            ? throw new TaskNotFoundException(query.TaskId)
            : TaskItemDto.FromDomain(taskItem);
    }
}
