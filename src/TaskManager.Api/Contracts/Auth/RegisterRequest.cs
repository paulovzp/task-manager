using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts.Auth;

/// <summary>Represents a user registration request.</summary>
public sealed class RegisterRequest
{
    /// <summary>Gets or sets the user's display name.</summary>
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's email.</summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's password.</summary>
    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
