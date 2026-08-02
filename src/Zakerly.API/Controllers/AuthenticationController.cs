using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zakerly.Application.Features.Authentication.Login;
using Zakerly.Application.Features.Authentication.Register;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    // GET: api/auth/me
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var email = User.FindFirstValue(ClaimTypes.Email);

        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Role = role
        });
    }
}