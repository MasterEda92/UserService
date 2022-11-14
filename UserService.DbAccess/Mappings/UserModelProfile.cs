using AutoMapper;
using UserService.Core.Entities;
using UserService.DbAccess.Models;

namespace UserService.DbAccess.Mappings;

public class UserModelProfile : Profile
{
    public UserModelProfile()
    {
        CreateMap<User, UserModel>();
        CreateMap<UserModel, User>();
    }
}