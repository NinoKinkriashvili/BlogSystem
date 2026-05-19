using System.Security.Claims;
using BlogSystem.Application.DTOs.Post;
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


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _postService.GetAllAsync(page, pageSize, ct);
        return Ok(result);
    }


    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string? search,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _postService.SearchAsync(search, page, pageSize, ct);
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


    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var result = await _postService.CreateAsync(dto, userId, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto dto, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var result = await _postService.UpdateAsync(id, dto, userId, ct);

        return Ok(result);
    }


    [Authorize(Roles = "User,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _postService.DeleteAsync(id, userId, ct);

        return NoContent();
    }
}
