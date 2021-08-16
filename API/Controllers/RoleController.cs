using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Role;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService service)
        {
            _roleService = service;
        }
       
        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <response code="200">Returns all roles</response>
        /// <returns>Collection of roles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ICollection<RoleResponseDto>> GetRoleList()
        {
            var roles = _roleService.GetRoleList().Result;
            return roles;
        }

        /// <summary>
        /// Gets role by id
        /// </summary>
        /// <response code="200">Returns a role</response>
        /// <response code="404">If the role is not exists</response>
        /// <returns>role</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleResponseDto>> GetRoleById(int id)
        {
            var role = _roleService.GetRoleById(id).Result;
            
            if (role == null) return NotFound();
            
            return role;
        }

        /// <summary>
        /// Creates a role
        /// </summary>
        /// <param name="roleDto"></param>
        /// /// <response code="201">Returns the newly created role</response>
        /// <response code="400">If the role is null</response>
        /// <returns>A newly created role</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<RoleRequestDto>> CreateRole(RoleRequestDto roleDto)
        {
            var role = _roleService.GetRoleById(roleDto.Id).Result;

            if (role != null) return NoContent();
            
             _roleService.CreateRole(roleDto);
            return CreatedAtAction("GetRoleById", new {id = roleDto.Id}, roleDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = _roleService.GetRoleById(id).Result;

            if (role == null) return NotFound();
            
            _roleService.DeleteRole(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleRequestDto roleDto)
        {
            var role = await _roleService.GetRoleById(roleDto.Id);
            
            if (role == null) return NotFound();
            
            _roleService.UpdateRole(roleDto);
            return NoContent();
        }
    }
}