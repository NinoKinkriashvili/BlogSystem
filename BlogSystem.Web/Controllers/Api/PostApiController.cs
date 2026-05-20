using System.Security.Claims;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Web.Controllers.Api;

[ApiController]
[Route("api/posts")]
public class PostApiController : ControllerBase
{
    private readonly IPostService _postService;

    public PostApiController(IPostService postService)
    {
        _postService = postService;
    }

    // PUBLIC / USER ENDPOINTS

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingDto paging, CancellationToken ct = default)
    {
        var result = await _postService.GetAllAsync(paging.Page, paging.PageSize, ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? search, [FromQuery] PagingDto paging, CancellationToken ct = default)
    {
        var result = await _postService.SearchAsync(search, paging.Page, paging.PageSize, ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _postService.GetByIdAsync(id, ct);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // USER / ADMIN (CREATE)

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _postService.CreateAsync(dto, userId.Value, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // USER / ADMIN (UPDATE)

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _postService.UpdateAsync(id, dto, userId.Value, ct);

        return Ok(result);
    }

    // USER / ADMIN (DELETE)

    [Authorize(Roles = "User,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _postService.DeleteAsync(id, userId.Value, ct);

        return NoContent();
    }

    // HELPERS

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return null;

        return Guid.Parse(userIdClaim);
    }
}
