using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.Core.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var allUsers = await _userService.GetAllUsers();
        if (allUsers.Any())
            return Ok(allUsers);
        else
            return NotFound();
    }
}