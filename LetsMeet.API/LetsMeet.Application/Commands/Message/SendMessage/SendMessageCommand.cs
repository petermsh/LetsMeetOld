using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.Message.SendMessage;

public record SendMessageCommand(string RoomId, string Content) : ICommand<string>;