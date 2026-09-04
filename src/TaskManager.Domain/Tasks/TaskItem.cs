namespace TaskManager.Domain.Tasks;

/// <summary>
/// Represents a piece of work owned by a user.
/// </summary>
public sealed class TaskItem
{
    private const int MaxTitleLength = 200;
    private const int MaxDescriptionLength = 2_000;

    private TaskItem(
        Guid id,
        Guid ownerId,
        string title,
        string description,
        DateTimeOffset dueDate,
        TaskItemStatus status)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
    }

    /// <summary>
    /// Gets the task identifier.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the identifier of the user who owns the task.
    /// </summary>
    public Guid OwnerId { get; }

    /// <summary>
    /// Gets the task title.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the task description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the task due date.
    /// </summary>
    public DateTimeOffset DueDate { get; private set; }

    /// <summary>
    /// Gets the current task status.
    /// </summary>
    public TaskItemStatus Status { get; private set; }

    /// <summary>
    /// Changes the lifecycle status of the task.
    /// </summary>
    /// <param name="status">The new task status.</param>
    public void ChangeStatus(TaskItemStatus status)
    {
        EnsureCanBeChanged();
        Status = status;
    }

    /// <summary>
    /// Updates the editable task details.
    /// </summary>
    /// <param name="title">The new task title.</param>
    /// <param name="description">The new task description.</param>
    /// <param name="dueDate">The new task due date.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is missing.</exception>
    public void UpdateDetails(string title, string description, DateTimeOffset dueDate)
    {
        EnsureCanBeChanged();

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A task title is required.", nameof(title));
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ArgumentException(
                $"A task title cannot exceed {MaxTitleLength} characters.",
                nameof(title));
        }

        if (description.Length > MaxDescriptionLength)
        {
            throw new ArgumentException(
                $"A task description cannot exceed {MaxDescriptionLength} characters.",
                nameof(description));
        }

        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    private void EnsureCanBeChanged()
    {
        if (Status == TaskItemStatus.Completed)
        {
            throw new CompletedTaskCannotBeChangedException();
        }
    }

    /// <summary>
    /// Creates a pending task for a user.
    /// </summary>
    /// <param name="ownerId">The identifier of the user who owns the task.</param>
    /// <param name="title">The task title.</param>
    /// <param name="description">The task description.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <returns>A new pending task item.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="ownerId"/> does not identify a user or when
    /// <paramref name="title"/> is missing or a text field exceeds its allowed length.
    /// </exception>
    public static TaskItem Create(
        Guid ownerId,
        string title,
        string description,
        DateTimeOffset dueDate)
    {
        if (ownerId == Guid.Empty)
        {
            throw new ArgumentException("A task owner is required.", nameof(ownerId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A task title is required.", nameof(title));
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ArgumentException(
                $"A task title cannot exceed {MaxTitleLength} characters.",
                nameof(title));
        }

        if (description.Length > MaxDescriptionLength)
        {
            throw new ArgumentException(
                $"A task description cannot exceed {MaxDescriptionLength} characters.",
                nameof(description));
        }

        return new TaskItem(
            Guid.NewGuid(),
            ownerId,
            title,
            description,
            dueDate,
            TaskItemStatus.Pending);
    }
}
