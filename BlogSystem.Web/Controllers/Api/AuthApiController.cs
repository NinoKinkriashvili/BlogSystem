using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(dto, ct);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(dto, ct);
        return CreatedAtAction(nameof(Login), result);
    }
}
