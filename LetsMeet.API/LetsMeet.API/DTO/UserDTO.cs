using FluentValidation;
using LetsMeet.API.Database.Entities;

namespace LetsMeet.API.DTO;

public class UserRegDto
{
    public string Email { get; set; }
    public string Nick { get; set; }
    public string Password { get; set; }
    public string ReapetedPassword { get; set; }
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
}

public class UserLoginDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}

public class UserInfoDto
{
    public string Nick { get; set; }
    public string Bio { get; set; }
    public string City { get; set; }
    public string University { get; set; }
    public string Major { get; set; }
}

public class FindUserDto
{
    public string Nick { get; set; }
    public LastMessageDto LastMessage { get; set; }
    public string RoomId { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public class UserEditDto
{
    public string? Nick { get; set; }
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? University { get; set; }
}

public class UserRegDtoValidator : AbstractValidator<UserRegDto>
{
    private const string rule = @"^[a-zA-Z0-9_\-\.]+$";

    public UserRegDtoValidator()
    {
        RuleFor(user => user.Nick).MinimumLength(4).MaximumLength(64).NotEmpty()
            .Matches(rule).WithMessage("Dozwolone litery, cyfry oraz znaki: _ - .");
        RuleFor(user => user.Email).EmailAddress().NotEmpty();
        RuleFor(user => user.Password).MaximumLength(128).MinimumLength(8).NotEmpty();
        RuleFor(user => user.Bio).MaximumLength(512);
    }
}