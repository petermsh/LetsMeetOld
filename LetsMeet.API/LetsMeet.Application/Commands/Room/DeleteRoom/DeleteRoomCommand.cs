using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Commands.Room.DeleteRoom;

public record DeleteRoomCommand(string RoomId) : ICommand;