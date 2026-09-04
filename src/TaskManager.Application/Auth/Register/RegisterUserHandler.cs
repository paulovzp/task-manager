using TaskManager.Application.Abstractions;

namespace TaskManager.Application.Auth.Register;

/// <summary>Registers application users.</summary>
public sealed class RegisterUserHandler(IIdentityService identityService)
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));

    /// <summary>Handles a registration request.</summary>
    public Task<AuthResult> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return _identityService.RegisterAsync(
            command.Name,
            command.Email,
            command.Password,
            cancellationToken);
    }
}
