namespace TaskManager.Application.Auth.Register;

/// <summary>Requests creation of an application user.</summary>
/// <param name="Name">The user's display name.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public sealed record RegisterUserCommand(string Name, string Email, string Password);
