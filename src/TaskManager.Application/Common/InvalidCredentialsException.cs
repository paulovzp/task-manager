namespace TaskManager.Application.Common;

/// <summary>Represents an authentication attempt with invalid credentials.</summary>
public sealed class InvalidCredentialsException() : Exception("Email or password is invalid.");
