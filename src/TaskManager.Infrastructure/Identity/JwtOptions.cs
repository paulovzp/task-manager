using System.ComponentModel.DataAnnotations;

namespace TaskManager.Infrastructure.Identity;

/// <summary>Defines JWT creation and validation settings.</summary>
public sealed class JwtOptions
{
    /// <summary>Gets or sets the token issuer.</summary>
    [Required]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Gets or sets the token audience.</summary>
    [Required]
    public string Audience { get; set; } = string.Empty;

    /// <summary>Gets or sets the symmetric signing key.</summary>
    [Required, MinLength(32)]
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>Gets or sets the token lifetime in minutes.</summary>
    [Range(5, 1_440)]
    public int ExpirationMinutes { get; set; } = 60;
}
