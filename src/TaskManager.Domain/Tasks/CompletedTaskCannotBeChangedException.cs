namespace TaskManager.Domain.Tasks;

/// <summary>Represents an attempt to change a completed task.</summary>
public sealed class CompletedTaskCannotBeChangedException()
    : InvalidOperationException("Completed tasks cannot be changed.");
