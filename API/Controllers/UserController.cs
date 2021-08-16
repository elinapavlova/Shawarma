
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.User;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }
       
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <response code="200">Returns all users</response>
        /// <returns>Collection of users</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ICollection<UserResponseDto>> GetUserList()
        {
            var users = _userService.GetUserList().Result;
            return users;
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <response code="200">Returns a user</response>
        /// <response code="404">If the user is not exists</response>
        /// <returns>User</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id).Result;
            
            if (user == null) return NotFound();
            
            return user;
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userDto"></param>
        /// /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is null</response>
        /// <response code="500">If something is wrong</response>
        /// <response code="500">If user with this Id exists</response>
        /// <returns>A newly created user</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<UserRequestDto>> CreateUser(UserRequestDto userDto)
        {
            if (userDto == null)  
                return BadRequest();

            var user = _userService.GetUserById(userDto.Id).Result;
            
            if (user != null)
                return NoContent();

            _userService.CreateUser(userDto);
            return CreatedAtAction("GetUserById", new {id = userDto.Id}, userDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.GetUserById(id).Result;

            if (user == null) 
                return NotFound();
            
            _userService.DeleteUser(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserRequestDto userDto)
        {
            var user = await _userService.GetUserById(userDto.Id);
            
            if (user == null) 
                return NotFound();
            
            _userService.UpdateUser(userDto);
            return NoContent();
        }
    }
}