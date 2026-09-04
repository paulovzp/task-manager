using TaskManager.Application.Abstractions;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Create;

/// <summary>
/// Creates task items and persists them.
/// </summary>
public sealed class CreateTaskHandler(ITaskRepository repository, TimeProvider timeProvider)
{
    private readonly ITaskRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    private readonly TimeProvider _timeProvider =
        timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    /// <summary>Handles a create-task request.</summary>
    public async Task<TaskItemDto> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.DueDate <= _timeProvider.GetUtcNow())
        {
            throw new ArgumentException("The due date must be in the future.", nameof(command.DueDate));
        }

        var taskItem = TaskItem.Create(
            command.OwnerId,
            command.Title,
            command.Description,
            command.DueDate);

        await _repository.AddAsync(taskItem, cancellationToken).ConfigureAwait(false);
        await _repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return TaskItemDto.FromDomain(taskItem);
    }
}
