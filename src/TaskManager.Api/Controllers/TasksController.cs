using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts.Tasks;
using TaskManager.Application.Tasks.Create;
using TaskManager.Application.Tasks.Delete;
using TaskManager.Application.Tasks.Get;
using TaskManager.Application.Tasks.List;
using TaskManager.Application.Tasks.Update;

namespace TaskManager.Api.Controllers;

/// <summary>Exposes CRUD operations for authenticated users' tasks.</summary>
[Authorize]
[ApiController]
[Route("api/tasks")]
public sealed class TasksController(
    CreateTaskHandler createHandler,
    ListTasksHandler listHandler,
    GetTaskHandler getHandler,
    UpdateTaskHandler updateHandler,
    DeleteTaskHandler deleteHandler) : ControllerBase
{
    /// <summary>Lists tasks owned by the current user.</summary>
    [HttpGet]
    public async Task<IActionResult> ListAsync(CancellationToken cancellationToken) =>
        Ok(await listHandler.HandleAsync(new ListTasksQuery(GetUserId()), cancellationToken));

    /// <summary>Returns one task owned by the current user.</summary>
    [HttpGet("{id:guid}", Name = "GetTask")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken) =>
        Ok(await getHandler.HandleAsync(new GetTaskQuery(id, GetUserId()), cancellationToken));

    /// <summary>Creates a task for the current user.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await createHandler.HandleAsync(
            new CreateTaskCommand(GetUserId(), request.Title, request.Description, request.DueDate),
            cancellationToken);
        return CreatedAtRoute("GetTask", new { id = result.Id }, result);
    }

    /// <summary>Updates one task owned by the current user.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateHandler.HandleAsync(
            new UpdateTaskCommand(
                id,
                GetUserId(),
                request.Title,
                request.Description,
                request.Status,
                request.DueDate),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>Deletes one task owned by the current user.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await deleteHandler.HandleAsync(new DeleteTaskCommand(id, GetUserId()), cancellationToken);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new InvalidOperationException("The authenticated user identifier is invalid.");
    }
}
