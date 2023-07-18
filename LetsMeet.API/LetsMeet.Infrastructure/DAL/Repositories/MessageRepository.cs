using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Repositories;

internal class MessageRepository : IMessageRepository
{
    private readonly LetsMeetDbContext _dbContext;
    private readonly DbSet<Message> _messages;

    public MessageRepository(LetsMeetDbContext dbContext)
    {
        _dbContext = dbContext;
        _messages = dbContext.Messages;
    }

    public async Task AddAsync(Message message)
    {
        await _messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Message message)
    {
        _messages.Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        _messages.Update(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task GetAsync(int id)
        => await _messages.SingleOrDefaultAsync(x => x.Id == id);
}