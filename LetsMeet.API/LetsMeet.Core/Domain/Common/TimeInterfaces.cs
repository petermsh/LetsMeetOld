namespace LetsMeet.Core.Domain.Common;

public interface ICreatedAt
{
    public DateTimeOffset CreatedAt { get; set; }
}

public interface IModifiedAt
{
    public DateTimeOffset? ModifiedAt { get; set; }
}