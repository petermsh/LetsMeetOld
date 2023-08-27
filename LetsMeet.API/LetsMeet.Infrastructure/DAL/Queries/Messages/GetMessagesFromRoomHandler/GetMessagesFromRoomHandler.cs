using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Queries.Messages.GetMessagesFromRoom;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Queries.Messages.GetMessagesFromRoomHandler;

internal sealed class GetMessagesFromRoomHandler : IQueryHandler<GetMessagesFromRoom, List<MessageDetailsDto>>
{
    private readonly LetsMeetDbContext _dbContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetMessagesFromRoomHandler(LetsMeetDbContext dbContext, IUserInfoProvider userInfoProvider)
    {
        _dbContext = dbContext;
        _userInfoProvider = userInfoProvider;
    }

    public async Task<List<MessageDetailsDto>> HandleAsync(GetMessagesFromRoom query)
    {
        var messages = await _dbContext.Messages.Where(x => x.RoomId == query.RoomId)
            .Select(q => new MessageDetailsDto()
            {
                Id = q.Id,
                From = q.SenderUserName,
                Content = q.Content,
                Date = q.CreatedAt,
                IsFromUser = _userInfoProvider.UserName == q.SenderUserName
            }).OrderBy(m => m.Date).ToListAsync();

        return messages;
    }
}