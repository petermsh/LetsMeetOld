using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Queries.User;
using LetsMeet.Application.Queries.User.GetUserByUserName;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Queries.GetUserByUserNameHandler;

internal sealed class GetUserByUserNameHandler : IQueryHandler<GetUserByUserNameQuery, UserDetailsDto>
{
    private readonly LetsMeetDbContext _dbContext;

    public GetUserByUserNameHandler(LetsMeetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDetailsDto> HandleAsync(GetUserByUserNameQuery query)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserName == query.UserName);

        if(user is null)
            throw new UserNotFoundException(query.UserName);
        
        return user.AsDto();
    }
}