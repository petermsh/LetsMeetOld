using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.Room;
using LetsMeet.Core.Domain.Repositories;

namespace LetsMeet.Application.Commands.Room.DeleteRoom;

public class DeleteRoomHandler : ICommandHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository _roomRepository;

    public DeleteRoomHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task HandleAsync(DeleteRoomCommand command)
    {
        var room = await _roomRepository.GetAsync(command.RoomId);
        if (room is null)
            throw new RoomNotFoundException();

        await _roomRepository.RemoveAsync(room);
    }
}