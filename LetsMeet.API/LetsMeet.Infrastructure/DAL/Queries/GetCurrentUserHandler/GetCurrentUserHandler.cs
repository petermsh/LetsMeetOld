using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Exceptions.User;
using LetsMeet.Application.Queries.User;
using LetsMeet.Application.Queries.User.GetCurrentUser;
using LetsMeet.Application.Queries.User.GetUserByUserName;
using LetsMeet.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Queries.GetCurrentUserHandler;

internal sealed class GetCurrentUserHandler : IQueryHandler<GetCurrentUserQuery, UserDetailsDto>
{
    private readonly LetsMeetDbContext _dbContext;

    public GetCurrentUserHandler(LetsMeetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDetailsDto> HandleAsync(GetCurrentUserQuery query)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserName == query.Username);

        if (user is null)
            throw new UserNotFoundException("");

        return user.AsDto();
    }
}