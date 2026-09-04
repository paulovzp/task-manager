using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Application.Tasks.Update;

/// <summary>Updates user-owned tasks.</summary>
public sealed class UpdateTaskHandler(ITaskRepository repository, TimeProvider timeProvider)
{
    private readonly ITaskRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    private readonly TimeProvider _timeProvider =
        timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    /// <summary>Handles an update-task request.</summary>
    public async Task<TaskItemDto> HandleAsync(
        UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.DueDate <= _timeProvider.GetUtcNow())
        {
            throw new ArgumentException("The due date must be in the future.", nameof(command.DueDate));
        }

        var taskItem = await _repository
            .GetByIdAsync(command.TaskId, command.OwnerId, cancellationToken)
            .ConfigureAwait(false);

        if (taskItem is null)
        {
            throw new TaskNotFoundException(command.TaskId);
        }

        taskItem.UpdateDetails(command.Title, command.Description, command.DueDate);
        taskItem.ChangeStatus(command.Status);
        await _repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return TaskItemDto.FromDomain(taskItem);
    }
}
