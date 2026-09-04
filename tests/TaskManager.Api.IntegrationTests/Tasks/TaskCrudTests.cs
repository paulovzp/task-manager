using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace TaskManager.Api.IntegrationTests.Tasks;

public sealed class TaskCrudTests : IClassFixture<TaskManagerApiFactory>
{
    private readonly HttpClient _client;

    public TaskCrudTests(TaskManagerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AuthenticatedUser_CanCompleteTaskCrud()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid():N}@example.com";
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Integration User",
            email,
            password = "Password1",
        });
        registerResponse.EnsureSuccessStatusCode();
        using var authDocument = JsonDocument.Parse(await registerResponse.Content.ReadAsStringAsync());
        var accessToken = authDocument.RootElement.GetProperty("accessToken").GetString();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Act: create
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", new
        {
            title = "Integration task",
            description = "Exercise the complete HTTP workflow.",
            dueDate = DateTimeOffset.UtcNow.AddDays(2),
        });

        // Assert: create
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        using var createdDocument = JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        var taskId = createdDocument.RootElement.GetProperty("id").GetGuid();

        // Act and assert: read
        var list = await _client.GetFromJsonAsync<JsonElement[]>("/api/tasks");
        Assert.Single(Assert.IsType<JsonElement[]>(list));

        // Act and assert: update
        var updateResponse = await _client.PutAsJsonAsync($"/api/tasks/{taskId}", new
        {
            title = "Updated integration task",
            description = "The workflow was updated.",
            status = "Completed",
            dueDate = DateTimeOffset.UtcNow.AddDays(3),
        });
        updateResponse.EnsureSuccessStatusCode();

        // Act and assert: delete
        var deleteResponse = await _client.DeleteAsync($"/api/tasks/{taskId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        var emptyList = await _client.GetFromJsonAsync<JsonElement[]>("/api/tasks");
        Assert.Empty(Assert.IsType<JsonElement[]>(emptyList));
    }
}
