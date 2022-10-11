using AutoMapper;
using AutoMapper.QueryableExtensions;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Exceptions;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Services;

internal class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IAuthManager _authManager;
    private readonly IMapper _mapper;
    private readonly IHashService _hashService;
    private readonly IUserInfoProvider _userInfoProvider;

    public UserService(DataContext context, IMapper mapper, IHashService hashService, IAuthManager authManager, IUserInfoProvider userInfoProvider)
    {
        _context = context;
        _mapper = mapper;
        _hashService = hashService;
        _authManager = authManager;
        _userInfoProvider = userInfoProvider;
    }
    
    public void CreateUser(UserRegDto userRegDto)
    {
        if (_context.Users.Any(x => x.Email == userRegDto.Email))
        {
            throw new UserEmailAlreadyExistException(userRegDto.Email);
        }

        if (_context.Users.Any(x => x.Nick == userRegDto.Nick))
        {
            throw new UserNameAlreadyExistException(userRegDto.Nick);
        }
        
        var user = _mapper.Map<User>(userRegDto);
        user.Password = _hashService.Hash(userRegDto.Password);
        
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public string Login(UserLoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Nick == dto.Login || x.Email == dto.Login);

        if (user is null)
            throw new UserNotFoundException(dto.Login);

        if (!_hashService.Check(user.Password, dto.Password))
            throw new UserWrongPasswordException();

        var token = _authManager.CreateToken(user);
        return token;
    }
    
    public UserInfoDto GetInfo()
    {
        var user = _userInfoProvider.CurrentUser;
        if (user is null)
            throw new UserNotFoundException("");

        var info = _mapper.Map<UserInfoDto>(user);

        return info;
    }

    public UserInfoDto GetUser(string nick)
    {
        var user = _context.Users.SingleOrDefault(x => x.Nick == nick);
        if (user is null)
            throw new UserNotFoundException("");
        var userInfo = _mapper.Map<UserInfoDto>(user);
        
        return userInfo;
    }

    public void ChangeStatus(bool status)
    {
        var user = _userInfoProvider.CurrentUser;
        if (user is null)
            throw new UserNotFoundException("");
        user.Status = status;

        _context.Update(user);
        _context.SaveChanges();
    }
}