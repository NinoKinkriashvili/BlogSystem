using BlogSystem.Application.DTOs.User;
using FluentValidation;

namespace BlogSystem.Application.Validators.User;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(user => user.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required");
    }
}
