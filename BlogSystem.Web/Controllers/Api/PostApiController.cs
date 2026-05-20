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

    // PUBLIC

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

    // AUTH

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken ct)
    {
        var result = await _postService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto dto, CancellationToken ct)
    {
        var result = await _postService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _postService.DeleteAsync(id, ct);
        return NoContent();
    }
}
