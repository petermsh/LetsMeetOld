namespace LetsMeet.Application.DTO.Account;

public class ChangePasswordDto
{
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }
    public string Token { get; init; }
}