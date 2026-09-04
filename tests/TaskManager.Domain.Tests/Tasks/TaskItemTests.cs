using TaskManager.Domain.Tasks;

namespace TaskManager.Domain.Tests.Tasks;

public sealed class TaskItemTests
{
    [Fact]
    public void UpdateDetails_DescriptionLongerThan2000Characters_ThrowsArgumentException()
    {
        var taskItem = TaskItem.Create(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Initial", "Initial", DateTimeOffset.UtcNow.AddDays(1));

        var exception = Assert.Throws<ArgumentException>(() => taskItem.UpdateDetails(
            "Updated", new string('a', 2001), DateTimeOffset.UtcNow.AddDays(2)));

        Assert.Equal("description", exception.ParamName);
    }

    [Fact]
    public void UpdateDetails_TitleLongerThan200Characters_ThrowsArgumentException()
    {
        var taskItem = TaskItem.Create(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Initial", "Initial", DateTimeOffset.UtcNow.AddDays(1));

        var exception = Assert.Throws<ArgumentException>(() => taskItem.UpdateDetails(
            new string('a', 201), "Updated", DateTimeOffset.UtcNow.AddDays(2)));

        Assert.Equal("title", exception.ParamName);
    }

    [Fact]
    public void ChangeStatus_ValidStatus_ChangesTaskStatus()
    {
        // Arrange
        var taskItem = TaskItem.Create(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Initial title",
            "Initial description",
            new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero));

        // Act
        taskItem.ChangeStatus(TaskItemStatus.InProgress);

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, taskItem.Status);
    }

    [Fact]
    public void UpdateDetails_MissingTitle_ThrowsArgumentException()
    {
        // Arrange
        var taskItem = TaskItem.Create(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Initial title",
            "Initial description",
            new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero));

        // Act
        var exception = Assert.Throws<ArgumentException>(() => taskItem.UpdateDetails(
            " ",
            "Updated description",
            new DateTimeOffset(2026, 9, 12, 18, 0, 0, TimeSpan.Zero)));

        // Assert
        Assert.Equal("title", exception.ParamName);
    }

    [Fact]
    public void UpdateDetails_ValidDetails_ChangesTaskData()
    {
        // Arrange
        var taskItem = TaskItem.Create(
            Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
            "Initial title",
            "Initial description",
            new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero));
        var updatedDueDate = new DateTimeOffset(2026, 9, 12, 18, 0, 0, TimeSpan.Zero);

        // Act
        taskItem.UpdateDetails("Updated title", "Updated description", updatedDueDate);

        // Assert
        Assert.Equal("Updated title", taskItem.Title);
        Assert.Equal("Updated description", taskItem.Description);
        Assert.Equal(updatedDueDate, taskItem.DueDate);
    }

    [Fact]
    public void Create_DescriptionLongerThan2000Characters_ThrowsArgumentException()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var dueDate = new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero);
        var description = new string('a', 2001);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => TaskItem.Create(
            ownerId,
            "Prepare technical presentation",
            description,
            dueDate));

        // Assert
        Assert.Equal("description", exception.ParamName);
    }

    [Fact]
    public void Create_TitleLongerThan200Characters_ThrowsArgumentException()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var dueDate = new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero);
        var title = new string('a', 201);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => TaskItem.Create(
            ownerId,
            title,
            "Review architecture decisions and rehearse the demo.",
            dueDate));

        // Assert
        Assert.Equal("title", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_MissingTitle_ThrowsArgumentException(string? invalidTitle)
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var dueDate = new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => TaskItem.Create(
            ownerId,
            invalidTitle!,
            "Review architecture decisions and rehearse the demo.",
            dueDate));

        // Assert
        Assert.Equal("title", exception.ParamName);
    }

    [Fact]
    public void Create_EmptyOwnerId_ThrowsArgumentException()
    {
        // Arrange
        var dueDate = new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => TaskItem.Create(
            Guid.Empty,
            "Prepare technical presentation",
            "Review architecture decisions and rehearse the demo.",
            dueDate));

        // Assert
        Assert.Equal("ownerId", exception.ParamName);
    }

    [Fact]
    public void Create_ValidDetails_CreatesPendingTask()
    {
        // Arrange
        var ownerId = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e");
        var dueDate = new DateTimeOffset(2026, 9, 10, 18, 0, 0, TimeSpan.Zero);

        // Act
        var taskItem = TaskItem.Create(
            ownerId,
            "Prepare technical presentation",
            "Review architecture decisions and rehearse the demo.",
            dueDate);

        // Assert
        Assert.NotEqual(Guid.Empty, taskItem.Id);
        Assert.Equal(ownerId, taskItem.OwnerId);
        Assert.Equal("Prepare technical presentation", taskItem.Title);
        Assert.Equal("Review architecture decisions and rehearse the demo.", taskItem.Description);
        Assert.Equal(dueDate, taskItem.DueDate);
        Assert.Equal(TaskItemStatus.Pending, taskItem.Status);
    }
}
