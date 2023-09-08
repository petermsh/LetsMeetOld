namespace LetsMeet.Application.DTO.Account;

public class ForgotPasswordDto
{
    public bool Status { get; init; }
    public string Message { get; init; }
    public string StatusCode { get; init; }
    public string Data { get; init; }
}