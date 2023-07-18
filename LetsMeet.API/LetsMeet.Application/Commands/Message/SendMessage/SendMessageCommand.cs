using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.Message.SendMessage;

public record SendMessageCommand(string Content, string RoomId) : ICommand<string>;