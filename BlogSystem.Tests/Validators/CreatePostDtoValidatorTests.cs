using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.Validators.Post;

using FluentValidation.TestHelper;

namespace BlogSystem.Tests.Validators;

public class CreatePostDtoValidatorTests
{
    private readonly CreatePostDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Model_Is_Valid()
    {
        var model = new CreatePostDto
        {
            Title = "How to learn C#",
            Content = "This is a long enough content for the post to pass validation."
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Title_Is_Empty()
    {
        var model = new CreatePostDto
        {
            Title = "",
            Content = "Valid content here that is long enough."
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Fail_When_Title_Too_Long()
    {
        var model = new CreatePostDto
        {
            Title = new string('A', 101),
            Content = "Valid content here that is long enough."
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Fail_When_Content_Is_Empty()
    {
        var model = new CreatePostDto
        {
            Title = "Valid title",
            Content = ""
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Fail_When_Content_Too_Short()
    {
        var model = new CreatePostDto
        {
            Title = "Valid title",
            Content = "short"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}
