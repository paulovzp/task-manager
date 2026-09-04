namespace TaskManager.Application.Tasks.List;

/// <summary>Requests all task items owned by a user.</summary>
/// <param name="OwnerId">The owning user identifier.</param>
public sealed record ListTasksQuery(Guid OwnerId);
