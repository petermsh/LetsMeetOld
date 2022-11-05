using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsMeet.API.Interfaces;

public interface IUserService
{
    
    void Update(User user);
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByUsernameAsync(string username);
}