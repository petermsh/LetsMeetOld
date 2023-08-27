using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Room;
using LetsMeet.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Application.Commands.Room.CreateRoom;

public record CreateRoomCommand() : ICommand<CreatedRoomDto>
{
    public string? ConnectionId { get; set; }
    public bool IsUniversity { get; init; }
    public bool IsCity { get; init; }
    public Gender Gender { get; init; }
}