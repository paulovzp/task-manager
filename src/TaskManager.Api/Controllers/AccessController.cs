using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.Controllers;

/// <summary>Demonstrates public and protected API access.</summary>
[ApiController]
[Route("api")]
public sealed class AccessController : ControllerBase
{
    /// <summary>Returns publicly available service information.</summary>
    [AllowAnonymous]
    [HttpGet("public/info")]
    public IActionResult PublicInfo() => Ok(new { Message = "Task Manager API is available." });

    /// <summary>Returns information only to authenticated users.</summary>
    [Authorize]
    [HttpGet("secure/info")]
    public IActionResult SecureInfo() => Ok(new { Message = "You are authenticated." });
}
