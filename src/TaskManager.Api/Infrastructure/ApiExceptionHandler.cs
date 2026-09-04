using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;

namespace TaskManager.Api.Infrastructure;

/// <summary>Converts expected application exceptions into problem details.</summary>
public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            TaskNotFoundException => (StatusCodes.Status404NotFound, "Task not found"),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Invalid credentials"),
            IdentityOperationException => (StatusCodes.Status400BadRequest, "Registration failed"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Validation failed"),
            _ => (StatusCodes.Status500InternalServerError, "Unexpected error"),
        };

        if (status >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled request error for {Path}", httpContext.Request.Path);
        }

        var details = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status < 500 ? exception.Message : "An unexpected error occurred.",
            Instance = httpContext.Request.Path,
        };
        if (exception is IdentityOperationException identityException)
        {
            details.Extensions["errors"] = identityException.Errors;
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
