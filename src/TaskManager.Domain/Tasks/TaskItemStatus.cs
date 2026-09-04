namespace TaskManager.Domain.Tasks;

/// <summary>
/// Represents the lifecycle state of a task item.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>
    /// The task has not been started.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Work on the task is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The task has been completed.
    /// </summary>
    Completed = 2,
}
