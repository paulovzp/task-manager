using Microsoft.AspNetCore.Identity;

namespace TaskManager.Infrastructure.Identity;

/// <summary>Represents an application user persisted by ASP.NET Core Identity.</summary>
public sealed class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>Gets or sets the user's display name.</summary>
    public string Name { get; set; } = string.Empty;
}
