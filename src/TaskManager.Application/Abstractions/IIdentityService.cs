namespace TaskManager.Application.Abstractions;

/// <summary>Provides registration and authentication operations.</summary>
public interface IIdentityService
{
    /// <summary>Creates a user and returns an access token.</summary>
    Task<AuthResult> RegisterAsync(
        string name,
        string email,
        string password,
        CancellationToken cancellationToken);

    /// <summary>Authenticates a user and returns an access token.</summary>
    Task<AuthResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken);
}

/// <summary>Represents an authenticated user and their access token.</summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="Name">The display name.</param>
/// <param name="Email">The email address.</param>
/// <param name="AccessToken">The JWT access token.</param>
/// <param name="ExpiresAt">The token expiration time.</param>
public sealed record AuthResult(
    Guid UserId,
    string Name,
    string Email,
    string AccessToken,
    DateTimeOffset ExpiresAt);
