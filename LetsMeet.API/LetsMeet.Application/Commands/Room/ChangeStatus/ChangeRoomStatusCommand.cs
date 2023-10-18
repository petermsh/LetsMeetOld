using ICommand = LetsMeet.Application.Abstractions.ICommand;

namespace LetsMeet.Application.Commands.Room.ChangeStatus;

public record ChangeRoomStatusCommand(string RoomId, bool Status) : ICommand;