namespace LetsMeet.API.Helper;

public class UserParams : PaginationParams
{
    public string CurrentUsername { get; set; }
    public string OrderBy { get; set; } = "lastActive";
}