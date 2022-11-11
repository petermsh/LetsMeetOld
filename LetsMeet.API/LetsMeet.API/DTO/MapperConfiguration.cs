using AutoMapper;
using LetsMeet.API.Database.Entities;

namespace LetsMeet.API.DTO;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);

        CreateMap<User, UserRegDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserInfoDto>().ReverseMap();
        CreateMap<User, UserEditDto>().ReverseMap();
        CreateMap<Message, SingleMessageDto>().ReverseMap();
    }
}