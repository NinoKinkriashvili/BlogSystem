namespace BlogSystem.Web.Models.Post;

public class PostItemViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }
}
