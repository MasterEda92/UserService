using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.DTOs;
using UserService.Core.Entities;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>?>> GetAllUsers()
    {
        var allUsers = await _userService.GetAllUsers();
        if (allUsers.Any())
        {
            var users = _mapper.Map<List<UserDto>>(allUsers);
            return Ok(users);
        }
        else
            return NotFound();
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserById([FromRoute]int userId)
    {
        var user = await GetUserByIdIfUserExistsElseNull(userId);
        if (user is null)
            return NotFound();
        
        return Ok(_mapper.Map<UserDto>(user));
    }

    private async Task<User?> GetUserByIdIfUserExistsElseNull(int userId)
    {
        try
        {
            var user = await _userService.GetUserById(userId)!;
            return user;
        }
        catch (UserNotFoundException)
        {
            return null;
        }
    }

    [HttpPost("/register")]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody]RegisterUserDto newUser)
    {
        throw new NotImplementedException();
    }
}