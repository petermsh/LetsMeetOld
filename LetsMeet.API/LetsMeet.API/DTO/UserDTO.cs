using System.ComponentModel.DataAnnotations;
using FluentValidation;
using LetsMeet.API.Database.Entities;

namespace LetsMeet.API.DTO;

public class UserRegDto
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ReapetedPassword { get; set; }
    public string? Bio { get; set; }
    public int Gender { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
}

public class UserReturnDto
{
    public string UserName { get; set; }
    public string Token { get; set; }
}

public class UserLoginDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}

public class UserInfoDto
{
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string City { get; set; }
    public string University { get; set; }
    public string Major { get; set; }
}

public class DrawUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
}

public class UserEditDto
{
    public string UserName { get; set; }
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public int Gender { get; set; }
}

public class ForgotPasswordDto
{
    public string Email { get; init; }
}

public class ChangePasswordDto
{
    public string OldPassword { get; init; }
    public string NewPassword { get; init; }
    public string ReapetedNewPassword { get; init; }
}

public class ResetPasswordDto
{
    public string Id { get; init; }
    public string NewPassword { get; init; }
    //public string ConfirmNewPassword { get; init; }
    public string Token { get; init; }
}

public class ResetPasswordDto2
{
    public string Email { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }
    public string Token { get; init; }
}

public class UserEditDtoValidator : AbstractValidator<UserEditDto>
{
    public UserEditDtoValidator()
    {
        RuleFor(user => user.Bio).MaximumLength(512);
    }
}

public class UserRegDtoValidator : AbstractValidator<UserRegDto>
{
    private const string rule = @"^[a-zA-Z0-9_\-\.]+$";

    public UserRegDtoValidator()
    {
        RuleFor(user => user.UserName).MinimumLength(4).MaximumLength(64).NotEmpty()
            .Matches(rule).WithMessage("Dozwolone litery, cyfry oraz znaki: _ - .");
        RuleFor(user => user.Email).EmailAddress().NotEmpty();
        RuleFor(user => user.Password).MaximumLength(128).MinimumLength(8).NotEmpty();
        RuleFor(user => user.Bio).MaximumLength(512);
    }
}