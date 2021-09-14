using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.Role;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly int _pageSize;
        public RoleController
        (
            IRoleService service,
            IConfiguration configuration
        )
        {
            _roleService = service;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }
       
        /// <summary>
        /// Get all roles
        /// </summary>
        /// <response code="200">Returns all roles</response>
        /// <returns>Collection of roles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultContainer<ICollection<RoleResponseDto>>>> GetRoleList(int page = 1)
        {
            return await ReturnResult<ResultContainer<ICollection<RoleResponseDto>>, ICollection<RoleResponseDto>>
                (_roleService.GetListByPage(_pageSize, page));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <response code="200">Returns the role</response>
        /// <response code="404">If the role does not exists</response>
        /// <returns>role</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleResponseDto>> GetRoleById(int id)
        {
            return await ReturnResult<ResultContainer<RoleResponseDto>, RoleResponseDto>
                (_roleService.GetById(id));
        }

        /// <summary>
        /// Create a role
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
            return await ReturnResult<ResultContainer<RoleResponseDto>, RoleResponseDto>
                (_roleService.Create(roleDto));
        }

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If the role was deleted successfully</response>
        /// <response code="404">If the role does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            return await ReturnResult<ResultContainer<RoleResponseDto>, RoleResponseDto>
                (_roleService.Delete(id));
        }
        
        /// <summary>
        /// Update the role
        /// </summary>
        /// <response code="204">If the role was updated successfully</response>
        /// <response code="400">If the role is null</response>
        /// <response code="404">If the role does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleRequestDto roleDto)
        {
            return await ReturnResult<ResultContainer<RoleResponseDto>, RoleResponseDto>
                (_roleService.Edit(roleDto));
        }
    }
}