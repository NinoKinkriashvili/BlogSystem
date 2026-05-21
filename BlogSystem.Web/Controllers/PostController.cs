using System.Security.Claims;
using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Web.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers;

[Route("posts")]
[ApiExplorerSettings(IgnoreApi = true)]
[Authorize(AuthenticationSchemes = "Cookies")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null, CancellationToken ct = default)
    {
        var result = string.IsNullOrWhiteSpace(search)
            ? await _postService.GetAllAsync(page, pageSize, ct)
            : await _postService.SearchAsync(search, page, pageSize, ct);

        return View(result);
    }

    [HttpGet("{id:guid}/{slug?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id, string? source = null, CancellationToken ct = default)
    {
        var post = await _postService.GetByIdAsync(id, ct);
        if (post == null) return NotFound();

        ViewBag.Source = source;
        SetBackInfo(source);
        var model = _mapper.Map<PostDetailsViewModel>(post);
        return View(model);
    }

    [HttpGet("my")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> MyPosts(int page = 1, int pageSize = 10, string? search = null, CancellationToken ct = default)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
            return Challenge();

        var userId = Guid.Parse(userIdStr);
        var result = await _postService.GetByUserIdAsync(userId, search, page, pageSize, ct);

        ViewData["IsMyPosts"] = true;
        return View("Index", result);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var dto = _mapper.Map<CreatePostDto>(model);
            dto.AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.CreateAsync(dto, ct);
            return RedirectToAction(nameof(CreateSuccess));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet("create/success")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult CreateSuccess()
    {
        return View();
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, string? source = null, CancellationToken ct = default)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Challenge();

        if (!User.IsInRole("User") && !User.IsInRole("Admin"))
            return Forbid();

        var post = await _postService.GetByIdAsync(id, ct);
        if (post == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && post.AuthorId.ToString() != userId)
            return Forbid();

        ViewBag.Source = source;
        var model = _mapper.Map<EditPostViewModel>(post);
        return View(model);
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditPostViewModel model, CancellationToken ct)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Challenge();

        if (!User.IsInRole("User") && !User.IsInRole("Admin"))
            return Forbid();

        var post = await _postService.GetByIdAsync(id, ct);
        if (post == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && post.AuthorId.ToString() != userId)
            return Forbid();

        if (!ModelState.IsValid) return View(model);

        try
        {
            var dto = _mapper.Map<UpdatePostDto>(model);
            await _postService.UpdateAsync(id, dto, ct);
            return RedirectToAction(nameof(EditSuccess), new { source = Request.Form["source"].FirstOrDefault() });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet("edit/success")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult EditSuccess(string? source = null)
    {
        ViewBag.Source = source;
        SetBackInfo(source);
        return View();
    }

    [HttpPost("delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, string? source = null, CancellationToken ct = default)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Challenge();

        if (!User.IsInRole("User") && !User.IsInRole("Admin"))
            return Forbid();

        var post = await _postService.GetByIdAsync(id, ct);
        if (post == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && post.AuthorId.ToString() != userId)
            return Forbid();

        await _postService.DeleteAsync(id, ct);
        return RedirectToAction(nameof(DeleteSuccess), new { source });
    }

    [HttpGet("delete/success")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult DeleteSuccess(string? source = null)
    {
        ViewBag.Source = source;
        SetBackInfo(source);
        return View();
    }

    private void SetBackInfo(string? source)
    {
        ViewBag.BackController = "Post";
        ViewBag.BackAction = "Index";
        ViewBag.BackText = "← Back to Posts";

        if (source == "my")
        {
            ViewBag.BackAction = "MyPosts";
            ViewBag.BackText = "← Back to My Posts";
        }
        else if (source == "dashboard")
        {
            ViewBag.BackController = "Home";
            ViewBag.BackAction = "Index";
            ViewBag.BackText = "← Back to Dashboard";
        }
    }
}
