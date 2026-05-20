using BlogSystem.Application.DTOs.Shared;
using FluentValidation;

namespace BlogSystem.Application.Validators.Shared;

public class PagingDtoValidator : AbstractValidator<PagingDto>
{
    public PagingDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
