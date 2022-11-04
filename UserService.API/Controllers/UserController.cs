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
    public async Task<ActionResult<IEnumerable<UserDto>?>> GetAllUsers()
    {
        var allUsers = await _userService.GetAllUsers();
        if (allUsers.Any())
        {
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    UserName = "Test",
                    Email = "test@test.com",
                    FirstName = "Max",
                    LastName = "Mustermann",
                    Password = "test"
                }
            };
            return Ok(users);
        }
        else
            return NotFound();
    }
}