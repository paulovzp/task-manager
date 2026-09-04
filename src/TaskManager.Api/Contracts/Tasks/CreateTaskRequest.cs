using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts.Tasks;

/// <summary>Represents a create-task request.</summary>
public sealed class CreateTaskRequest
{
    /// <summary>Gets or sets the task title.</summary>
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the task description.</summary>
    [Required, StringLength(2_000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets the due date.</summary>
    [Required]
    public DateTimeOffset DueDate { get; set; }
}
