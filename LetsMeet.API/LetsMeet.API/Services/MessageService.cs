using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Helper;
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
    
    public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername,
            string recipientUsername)
        {
            var messages = await _context.Messages
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername && m.SenderDeleted == false
                )
                .MarkUnreadAsRead(currentUsername)
                .OrderBy(m => m.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return messages;
        }
        
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                                            && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                                             && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };

            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);

        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
}