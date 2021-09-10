using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.User;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultContainer<ICollection<UserResponseDto>>>> GetUserList(int page = 1)
        {
            return await ReturnResult<ResultContainer<ICollection<UserResponseDto>>, ICollection<UserResponseDto>>
                (_userService.GetUserListByPage(2, page));
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
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                ( _userService.GetUserById(id));
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
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_userService.CreateUser(userDto));
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
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_userService.DeleteUser(id));
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
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_userService.UpdateUser(userDto));
        }
    }
}