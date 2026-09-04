using Microsoft.AspNetCore.Identity;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Common;

namespace TaskManager.Infrastructure.Identity;

/// <summary>Implements registration and authentication with ASP.NET Core Identity.</summary>
public sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService tokenService) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager =
        userManager ?? throw new ArgumentNullException(nameof(userManager));

    private readonly IJwtTokenService _tokenService =
        tokenService ?? throw new ArgumentNullException(nameof(tokenService));

    /// <inheritdoc />
    public async Task<AuthResult> RegisterAsync(
        string name,
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim(),
            UserName = email.Trim(),
        };
        var result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            throw new IdentityOperationException(result.Errors.Select(error => error.Description).ToArray());
        }

        return CreateResult(user);
    }

    /// <inheritdoc />
    public async Task<AuthResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.FindByEmailAsync(email.Trim()).ConfigureAwait(false);
        if (user is null || !await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
        {
            throw new InvalidCredentialsException();
        }

        return CreateResult(user);
    }

    private AuthResult CreateResult(ApplicationUser user)
    {
        var (token, expiresAt) = _tokenService.CreateToken(user);
        return new AuthResult(user.Id, user.Name, user.Email ?? string.Empty, token, expiresAt);
    }
}
