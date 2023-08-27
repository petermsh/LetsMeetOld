using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.Message;
using LetsMeet.Core.Domain.Repositories;

namespace LetsMeet.Application.Commands.Message.UpdateMessage;

internal sealed class UpdateMessageHandler : ICommandHandler<UpdateMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public UpdateMessageHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task HandleAsync(UpdateMessageCommand command)
    {
        var message = await _messageRepository.GetAsync(command.MessageId);

        if (message is null)
            throw new MessageNotFoundException();

        message.Content = command.Content;
        message.MessageSent = DateTime.UtcNow.ToString("O");

        await _messageRepository.UpdateAsync(message);
    }
}