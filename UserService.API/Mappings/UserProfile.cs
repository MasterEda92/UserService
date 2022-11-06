using AutoMapper;
using UserService.Core.DTOs;
using UserService.Core.Entities;

namespace UserService.API.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}