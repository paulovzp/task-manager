using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks;

/// <summary>
/// Represents task data returned by application use cases.
/// </summary>
/// <param name="Id">The task identifier.</param>
/// <param name="OwnerId">The owning user identifier.</param>
/// <param name="Title">The task title.</param>
/// <param name="Description">The task description.</param>
/// <param name="Status">The task status.</param>
/// <param name="DueDate">The task due date.</param>
public sealed record TaskItemDto(
    Guid Id,
    Guid OwnerId,
    string Title,
    string Description,
    TaskItemStatus Status,
    DateTimeOffset DueDate)
{
    /// <summary>Creates a DTO from a domain task item.</summary>
    public static TaskItemDto FromDomain(TaskItem taskItem) =>
        new(
            taskItem.Id,
            taskItem.OwnerId,
            taskItem.Title,
            taskItem.Description,
            taskItem.Status,
            taskItem.DueDate);
}
