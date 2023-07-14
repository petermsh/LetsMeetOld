using LetsMeet.Core.Domain.Enums;

namespace LetsMeet.Application.Queries.User.GetUserByUserName;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string City { get; set; }
    public string University { get; set; }
    public string Major { get; set; }
    public Gender Gender { get; set; }
}