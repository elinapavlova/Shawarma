
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
        private readonly IRoleService _roleService;

        public UserController(IUserService service, IRoleService roleService)
        {
            _userService = service;
            _roleService = roleService;
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
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUserById(long id)
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
            var status = _roleService.GetRoleById(userDto.IdRole).Result;

            if (status == null) 
                return NotFound();
            
            var user = _userService.GetUserById(userDto.Id).Result;

            if (user != null) 
                return NoContent();
            
            _userService.CreateUser(userDto);
            return CreatedAtAction("GetUserById", new {id = userDto.Id}, userDto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = _userService.GetUserById(id).Result;

            if (user == null) return NotFound();
            
            _userService.DeleteUser(id);
            return NoContent();
        }
        
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateUser(long id, UserRequestDto userDto)
        {
            var status = _roleService.GetRoleById(userDto.IdRole).Result;

            if (status == null) 
                return NotFound();
            
            if (id != userDto.Id) 
                return BadRequest();

            var user = await _userService.GetUserById(id);
            
            if (user == null) 
                return NotFound();
            
            _userService.UpdateUser(id, userDto);
            return NoContent();
        }
    }
}