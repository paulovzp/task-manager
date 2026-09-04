namespace TaskManager.Application.Tasks.Delete;

/// <summary>Requests removal of a user-owned task.</summary>
/// <param name="TaskId">The task identifier.</param>
/// <param name="OwnerId">The owning user identifier.</param>
public sealed record DeleteTaskCommand(Guid TaskId, Guid OwnerId);
