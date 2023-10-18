using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.Room;
using LetsMeet.Core.Domain.Repositories;

namespace LetsMeet.Application.Commands.Room.ChangeStatus;

public class ChangeRoomStatusHandler : ICommandHandler<ChangeRoomStatusCommand>
{
    private readonly IRoomRepository _roomRepository;

    public ChangeRoomStatusHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task HandleAsync(ChangeRoomStatusCommand command)
    {
        var room = await _roomRepository.GetAsync(command.RoomId);
        if (room is null)
            throw new RoomNotFoundException();

        room.IsLocked = command.Status;
        
        await _roomRepository.UpdateAsync(room);
    }
}