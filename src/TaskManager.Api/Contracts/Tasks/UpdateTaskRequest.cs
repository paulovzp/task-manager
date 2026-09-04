using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Tasks;

namespace TaskManager.Api.Contracts.Tasks;

/// <summary>Represents an update-task request.</summary>
public sealed class UpdateTaskRequest
{
    /// <summary>Gets or sets the task title.</summary>
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the task description.</summary>
    [Required, StringLength(2_000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets the task status.</summary>
    [Required]
    public TaskItemStatus Status { get; set; }

    /// <summary>Gets or sets the due date.</summary>
    [Required]
    public DateTimeOffset DueDate { get; set; }
}
