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
    #region FieldsAndCtor

    private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUserRegistrationValidator _registrationValidator;
        private readonly IUserUpdateValidator _userUpdateValidator;
    
        public UserController(IUserService userService, IMapper mapper, IUserRegistrationValidator registrationValidator,
            IUserUpdateValidator userUpdateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _registrationValidator = registrationValidator;
            _userUpdateValidator = userUpdateValidator;
        }

    #endregion

    #region Routes

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
    
        [HttpPost("/register")]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody]RegisterUserDto registerUser)
        {
            if (!_registrationValidator.ValidateUserData(registerUser))
                throw new UserRegistrationDataInvalidException();
            
            var user = await _userService.RegisterNewUser(registerUser);
            return StatusCode((int)HttpStatusCode.Created, _mapper.Map<UserDto>(user));
        }
    
        [HttpDelete("{userId:int}")]
        public async Task<ActionResult<UserDto>> DeleteUser(int userId)
        {
            if (!await _userService.CheckIfUserWithIdExists(userId))
                return StatusCode((int)HttpStatusCode.NotFound);
    
            var deletedUser = await DeletedAndGetUserWithIdElseNull(userId);
            if (deletedUser is not null)
                return Ok(_mapper.Map<UserDto>(deletedUser));
    
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    
        [HttpPut("{userId:int}")]
        public async Task<ActionResult<UserDto>> UpdateUser([FromRoute]int userId, [FromBody]UpdateUserDto updateUser)
        {
            if (!await _userService.CheckIfUserWithIdExists(userId))
                return StatusCode((int)HttpStatusCode.NotFound);
    
            if (!_userUpdateValidator.ValidateUserData(updateUser))
                return BadRequest();
    
            var user = await UpdateAndGetUserWithIdElseNull(userId, updateUser);
            if (user is not null)
                return Ok(_mapper.Map<UserDto>(user));
    
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

    #endregion

    #region PrivateFunctions

    private async Task<User?> UpdateAndGetUserWithIdElseNull(int userId, UpdateUserDto updateUser)
        {
            try
            {
                return await _userService.UpdateUserWithId(userId, updateUser);
            }
            catch (UserUpdateFailedException)
            {
                return null;
            }
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
        private async Task<User?> DeletedAndGetUserWithIdElseNull(int userId)
        {
            try
            {
                var deletedUser = await _userService.DeleteUserWithId(userId);
                return deletedUser;
            }
            catch (UserDeleteFailedException)
            {
                return null;
            }
        }

    #endregion

    
}