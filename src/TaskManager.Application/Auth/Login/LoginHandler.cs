using TaskManager.Application.Abstractions;

namespace TaskManager.Application.Auth.Login;

/// <summary>Authenticates application users.</summary>
public sealed class LoginHandler(IIdentityService identityService)
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));

    /// <summary>Handles a login request.</summary>
    public Task<AuthResult> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _identityService.LoginAsync(command.Email, command.Password, cancellationToken);
    }
}
