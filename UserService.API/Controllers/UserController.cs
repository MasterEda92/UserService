using System.Net;
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
    private readonly IUserValidator _validator;

    public UserController(IUserService userService, IMapper mapper, IUserValidator validator)
    {
        _userService = userService;
        _mapper = mapper;
        _validator = validator;
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
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody]RegisterUserDto registerUser)
    {
        if (!_validator.ValidateUserData(registerUser))
            throw new UserRegistrationDataInvalidException();
        
        var user = await _userService.RegisterNewUser(registerUser);
        var newUser = _mapper.Map<UserDto>(user);
        return StatusCode((int)HttpStatusCode.Created, newUser);
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult<UserDto>> DeleteUser(int userId)
    {
        bool isUserDeleted = await _userService.DeleteUserWithId(userId);
        if (isUserDeleted)
            return Ok(new UserDto());
        else
            return StatusCode((int)HttpStatusCode.InternalServerError);
    }

    [HttpPut("{userId:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute]int userId, [FromBody]UpdateUserDto updateUser)
    {
        var user = await _userService.UpdateUserWithId(userId, updateUser);
        return Ok(_mapper.Map<UserDto>(user));
    }
}