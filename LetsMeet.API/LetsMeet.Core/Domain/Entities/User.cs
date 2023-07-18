using LetsMeet.Core.Domain.Common;
using LetsMeet.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Core.Domain.Entities;

public class User : IdentityUser<Guid>, IModifiedAt, ICreatedAt
{
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public bool? Status { get; set; }
    public Gender? Gender { get; set; }
    public int MessageCount { get; set; } = 0;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    public ICollection<Room>? Rooms { get; set; }

    public Guid GetUserId()
    {
        return Id;
    }

    public void ChangeStatus(bool status) => Status = status;

    public void CountMessage() => MessageCount++;

}