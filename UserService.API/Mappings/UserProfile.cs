using AutoMapper;
using UserService.API.DTOs;
using UserService.Core.Entities;

namespace UserService.API.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}