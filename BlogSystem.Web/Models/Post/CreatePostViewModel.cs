using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Web.Models.Post;

public class CreatePostViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;
}
