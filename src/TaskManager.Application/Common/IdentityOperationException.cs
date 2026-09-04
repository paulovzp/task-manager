namespace TaskManager.Application.Common;

/// <summary>Represents a rejected Identity operation.</summary>
public sealed class IdentityOperationException(IReadOnlyList<string> errors)
    : Exception("The identity operation could not be completed.")
{
    /// <summary>Gets the validation errors returned by Identity.</summary>
    public IReadOnlyList<string> Errors { get; } = errors;
}
