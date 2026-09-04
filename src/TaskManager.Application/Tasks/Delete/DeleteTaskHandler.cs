using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.Tasks.Delete;

/// <summary>Deletes user-owned tasks.</summary>
public sealed class DeleteTaskHandler(ITaskRepository repository)
{
    private readonly ITaskRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>Handles a delete-task request.</summary>
    public async Task HandleAsync(
        DeleteTaskCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        var taskItem = await _repository
            .GetByIdAsync(command.TaskId, command.OwnerId, cancellationToken)
            .ConfigureAwait(false);

        if (taskItem is null)
        {
            throw new TaskNotFoundException(command.TaskId);
        }

        _repository.Remove(taskItem);
        await _repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
