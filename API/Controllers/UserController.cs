
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ICollection<UserResponseDto>> GetUserList()
        {
            var users = _userService.GetUserList().Result;
            return await Task.FromResult(users);
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If the user does not exists</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id).Result;
            
            if (user == null) return NotFound();
            
            return await Task.FromResult<ActionResult<UserResponseDto>>(user);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="204">If the user already exists</response>
        /// <response code="400">If the user is null or user role is not exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<UserRequestDto>> CreateUser(UserRequestDto userDto)
        {
            var user = _userService.GetUserById(userDto.Id).Result;
            var roleOfUser = _roleService.GetRoleById(userDto.IdRole).Result;
            
            if (user != null)
                return NoContent();            
            
            if (roleOfUser == null)  
                return BadRequest();
            
            _userService.CreateUser(userDto);
            return await Task.FromResult<ActionResult<UserRequestDto>>
                (CreatedAtAction("GetUserById", new {id = userDto.Id}, userDto));
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">If the user was deleted successfully</response>
        /// <response code="404">If the user does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.GetUserById(id).Result;

            if (user == null) 
                return NotFound();
            
            _userService.DeleteUser(id);
            return NoContent();
        }
        
        /// <summary>
        /// Update the user
        /// </summary>
        /// <response code="204">If the user was updated successfully</response>
        /// <response code="400">If the user is null or user role does not exists</response>
        /// <response code="404">If the user does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(UserRequestDto userDto)
        {
            var user = await _userService.GetUserById(userDto.Id);
            var roleOfUser = _roleService.GetRoleById(userDto.IdRole).Result;

            if (user == null) 
                return NotFound();
            
            if (roleOfUser == null)  
                return BadRequest();
            
            _userService.UpdateUser(userDto);
            return NoContent();
        }
    }
}