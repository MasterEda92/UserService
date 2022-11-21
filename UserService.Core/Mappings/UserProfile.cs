using AutoMapper;
using UserService.Contracts.DTOs;
using UserService.Core.Entities;

namespace UserService.Core.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<LoginUserDto, User>();
    }
}