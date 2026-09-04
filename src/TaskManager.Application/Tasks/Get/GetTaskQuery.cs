namespace TaskManager.Application.Tasks.Get;

/// <summary>Requests one task owned by a user.</summary>
/// <param name="TaskId">The task identifier.</param>
/// <param name="OwnerId">The owning user identifier.</param>
public sealed record GetTaskQuery(Guid TaskId, Guid OwnerId);
