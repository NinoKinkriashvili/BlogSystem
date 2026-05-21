using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Application.DTOs.Post;

namespace BlogSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        public HomeController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var authResult = await HttpContext.AuthenticateAsync("Cookies");
            if (authResult.Succeeded && authResult.Principal != null)
            {
                HttpContext.User = authResult.Principal;
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var pagedResult = await _postService.GetAllAsync(1, 6, HttpContext.RequestAborted);
                var recentPosts = pagedResult?.Items ?? new List<PostDto>();
                return View(recentPosts); // Dashboard
            }

            return View(new List<PostDto>()); // Guest
        }
    }
}
