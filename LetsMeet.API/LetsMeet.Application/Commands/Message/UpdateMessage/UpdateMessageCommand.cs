
using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.Message.UpdateMessage;

public record UpdateMessageCommand(int MessageId, string Content) : ICommand;