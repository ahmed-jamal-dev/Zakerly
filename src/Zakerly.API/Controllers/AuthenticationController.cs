using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Features.Authentication.Login;
using Zakerly.Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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


    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
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