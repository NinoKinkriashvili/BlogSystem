using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.Exceptions;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Web.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers;

[Route("posts")]
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

        var model = _mapper.Map<PostListViewModel>(result);
        model.Search = search;
        return View(model);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var post = await _postService.GetByIdAsync(id, ct);

        if (post == null)
            return NotFound();

        var model = _mapper.Map<PostDetailsViewModel>(post);
        return View(model);
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
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = _mapper.Map<CreatePostDto>(model);
            var userId = GetCurrentUserId();

            await _postService.CreateAsync(dto, userId, ct);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var post = await _postService.GetByIdAsync(id, ct);

        if (post == null)
            return NotFound();

        var model = _mapper.Map<EditPostViewModel>(post);
        return View(model);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditPostViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = _mapper.Map<UpdatePostDto>(model);
            var userId = GetCurrentUserId();

            await _postService.UpdateAsync(id, dto, userId, ct);
            return RedirectToAction(nameof(Index), "Post");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();

        await _postService.DeleteAsync(id, userId, ct);

        return RedirectToAction(nameof(Index), "Post");
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id :
            throw new UnauthorizedException("User is not authenticated.");
    }
}
