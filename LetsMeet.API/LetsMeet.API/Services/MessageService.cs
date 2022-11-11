using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Services;

public class MessageService : IMessageService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public MessageService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
}