namespace TaskManager.Application.Tasks.Create;

/// <summary>
/// Requests creation of a task for an authenticated user.
/// </summary>
/// <param name="OwnerId">The owning user identifier.</param>
/// <param name="Title">The task title.</param>
/// <param name="Description">The task description.</param>
/// <param name="DueDate">The task due date.</param>
public sealed record CreateTaskCommand(
    Guid OwnerId,
    string Title,
    string Description,
    DateTimeOffset DueDate);
