using BlogSystem.Web.Models.Shared;

namespace BlogSystem.Web.Models.Post;

public class PostListViewModel
{
    public List<PostItemViewModel> Items { get; set; } = new();

    public PaginationViewModel Pagination { get; set; } = new();

    public string? Search { get; set; }
}
