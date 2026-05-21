using Xunit;
using FluentValidation.TestHelper;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Validators.User;

namespace BlogSystem.Tests.Validators;

public class RegisterUserDtoValidatorTests
{
    private readonly RegisterUserDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Model_Is_Valid()
    {
        var model = new RegisterUserDto
        {
            FirstName = "Nino",
            LastName = "Kinkriashvili",
            UserName = "nino123",
            Email = "nino@test.com",
            Password = "Password1!"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_FirstName_Is_Empty()
    {
        var model = new RegisterUserDto
        {
            FirstName = "",
            LastName = "Test",
            UserName = "test",
            Email = "test@test.com",
            Password = "Password1!"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var model = new RegisterUserDto
        {
            FirstName = "Nino",
            LastName = "Test",
            UserName = "test",
            Email = "invalid-email",
            Password = "Password1!"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Username_Too_Short()
    {
        var model = new RegisterUserDto
        {
            FirstName = "Nino",
            LastName = "Test",
            UserName = "ab",
            Email = "test@test.com",
            Password = "Password1!"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Should_Fail_When_Password_Has_No_Uppercase()
    {
        var model = new RegisterUserDto
        {
            FirstName = "Nino",
            LastName = "Test",
            UserName = "testuser",
            Email = "test@test.com",
            Password = "password1!"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_Has_No_Special_Char()
    {
        var model = new RegisterUserDto
        {
            FirstName = "Nino",
            LastName = "Test",
            UserName = "testuser",
            Email = "test@test.com",
            Password = "Password1"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
