using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Services;

internal class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper, DataContext dataContext)
    {
        _mapper = mapper;
        _dataContext = dataContext;
    }
    
    public void Update(User user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _dataContext.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _dataContext.Users.FindAsync(username);
    }
}