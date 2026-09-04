namespace TaskManager.Application.Common;

/// <summary>Represents an attempt to access a missing or unowned task.</summary>
public sealed class TaskNotFoundException(Guid taskId)
    : Exception($"Task '{taskId}' was not found.")
{
    /// <summary>Gets the task identifier that was requested.</summary>
    public Guid TaskId { get; } = taskId;
}
