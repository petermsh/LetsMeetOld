using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Queries.Messages.GetMessagesFromRoom;

public record GetMessagesFromRoom(string RoomId) : IQuery<List<MessageDetailsDto>>;