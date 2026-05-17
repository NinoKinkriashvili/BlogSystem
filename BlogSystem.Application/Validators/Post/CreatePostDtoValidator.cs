using BlogSystem.Application.DTOs.Post;
using FluentValidation;

namespace BlogSystem.Application.Validators.Post;

public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidator()
    {
        RuleFor(post => post.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(post => post.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(100).WithMessage("Content must be at least 100 characters.")
            .MaximumLength(5000).WithMessage("Content cannot exceed 5000 characters.");
    }
}
