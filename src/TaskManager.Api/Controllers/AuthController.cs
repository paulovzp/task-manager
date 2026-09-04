using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts.Auth;
using TaskManager.Application.Auth.Login;
using TaskManager.Application.Auth.Register;

namespace TaskManager.Api.Controllers;

/// <summary>Exposes registration and authentication endpoints.</summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(RegisterUserHandler registerHandler, LoginHandler loginHandler)
    : ControllerBase
{
    /// <summary>Registers a user and returns an access token.</summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await registerHandler.HandleAsync(
            new RegisterUserCommand(request.Name, request.Email, request.Password),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>Authenticates a user and returns an access token.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await loginHandler.HandleAsync(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns claims for the authenticated user.</summary>
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        Name = User.FindFirstValue(ClaimTypes.Name),
        Email = User.FindFirstValue(ClaimTypes.Email),
    });
}
