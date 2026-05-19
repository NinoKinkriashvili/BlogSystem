using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        // TODO
        return RedirectToAction("Index", "Post");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public IActionResult Register(string email, string username, string password)
    {
        // TODO
        return RedirectToAction("Index", "Post");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // TODO
        return RedirectToAction("Login");
    }
}
