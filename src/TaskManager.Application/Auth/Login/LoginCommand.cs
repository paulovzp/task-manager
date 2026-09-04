namespace TaskManager.Application.Auth.Login;

/// <summary>Requests authentication of an application user.</summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public sealed record LoginCommand(string Email, string Password);
