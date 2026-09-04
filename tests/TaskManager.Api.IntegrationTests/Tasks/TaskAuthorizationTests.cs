using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TaskManager.Api.IntegrationTests.Tasks;

public sealed class TaskAuthorizationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskAuthorizationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_WithoutAccessToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks", CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
