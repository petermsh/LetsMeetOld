namespace LetsMeet.API.Database;

public record ChatUser(string UserId, string UserName);
public record RoomRequest(string Room);

public record InputMessage(
    string Message,
    string Room
);

public record OutputMessage(
    string Message,
    string UserName,
    string Room,
    DateTimeOffset SentAt
);

public record UserMessage(
    ChatUser ChatUser,
    string Message,
    string Room,
    DateTimeOffset SentAt
)
{
    public OutputMessage Output => new(Message, ChatUser.UserName, Room, SentAt);
}