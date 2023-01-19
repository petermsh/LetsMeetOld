using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;

namespace LetsMeet.API.Interfaces;

public interface IChatService
{
    public CreatedRoomDto DrawUser(bool isUniversity, bool isCity, int gender);
}