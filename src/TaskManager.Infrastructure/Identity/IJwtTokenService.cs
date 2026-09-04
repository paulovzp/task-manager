namespace TaskManager.Infrastructure.Identity;

/// <summary>Creates signed access tokens for application users.</summary>
public interface IJwtTokenService
{
    /// <summary>Creates a signed token and returns its expiration time.</summary>
    (string Token, DateTimeOffset ExpiresAt) CreateToken(ApplicationUser user);
}
