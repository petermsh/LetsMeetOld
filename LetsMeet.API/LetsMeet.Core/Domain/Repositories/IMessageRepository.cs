using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Core.Domain.Repositories;

public interface IMessageRepository
{ 
    Task AddAsync(Message message);
    Task RemoveAsync(Message message);
    Task UpdateAsync(Message message);
    Task GetAsync(int id);
}