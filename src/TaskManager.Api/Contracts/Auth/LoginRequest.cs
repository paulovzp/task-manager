using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts.Auth;

/// <summary>Represents a login request.</summary>
public sealed class LoginRequest
{
    /// <summary>Gets or sets the user's email.</summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's password.</summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
