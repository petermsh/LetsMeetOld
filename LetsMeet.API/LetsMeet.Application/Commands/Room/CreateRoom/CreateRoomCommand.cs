using System.ComponentModel.DataAnnotations;
using LetsMeet.Application.Abstractions;
using LetsMeet.Application.DTO.Room;
using LetsMeet.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.Application.Commands.Room.CreateRoom;

public record CreateRoomCommand() : ICommand<CreatedRoomDto>
{
    [Required]public string ConnectionId { get; set; }
    public string? University { get; init; }
    public string? City { get; init; }
    public string? Major { get; init; }
    public Gender Gender { get; init; }
}