using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers;

[Route("posts")]
public class PostController : Controller
{
    [HttpGet("")]
    public IActionResult Index(int page = 1, int pageSize = 10, string? search = null)
    {
        // TODO
        return View();
    }

    [HttpGet("{id:guid}")]
    public IActionResult Details(Guid id)
    {
        // TODO
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("create")]
    public IActionResult Create(string title, string content)
    {
        // TODO
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("edit/{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        // TODO
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("edit/{id:guid}")]
    public IActionResult Edit(Guid id, string title, string content)
    {
        // TODO
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("delete/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        // TODO
        return RedirectToAction(nameof(Index));
    }
}
