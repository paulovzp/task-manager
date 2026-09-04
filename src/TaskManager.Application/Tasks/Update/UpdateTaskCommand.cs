using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Update;

/// <summary>Requests changes to a user-owned task.</summary>
/// <param name="TaskId">The task identifier.</param>
/// <param name="OwnerId">The owning user identifier.</param>
/// <param name="Title">The new title.</param>
/// <param name="Description">The new description.</param>
/// <param name="Status">The new status.</param>
/// <param name="DueDate">The new due date.</param>
public sealed record UpdateTaskCommand(
    Guid TaskId,
    Guid OwnerId,
    string Title,
    string Description,
    TaskItemStatus Status,
    DateTimeOffset DueDate);
