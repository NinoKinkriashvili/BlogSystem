using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Web.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace BlogSystem.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = _mapper.Map<LoginDto>(model);

        var result = await _authService.LoginAsync(dto, ct);

        // TODO: JWT
        return RedirectToAction(nameof(PostController.Index), "Post");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = _mapper.Map<RegisterUserDto>(model);

            var result = await _authService.RegisterAsync(dto, ct);
            return RedirectToAction(nameof(PostController.Index), "Post");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // TODO: later clear cookie/session
        return RedirectToAction(nameof(Login), "Auth");
    }
}
