using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Status;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : BaseController
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService service)
        {
            _statusService = service;
        }
        
        /// <summary>
        /// Gets all statuses
        /// </summary>
        /// <response code="200">Returns all statuses</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultContainer<ICollection<StatusResponseDto>>>> GetStatusList()
        {
            return await ReturnResult<ResultContainer<ICollection<StatusResponseDto>>, ICollection<StatusResponseDto>>
                (_statusService.GetStatusList());
        }
        
        
        /// <summary>
        /// Gets status by id
        /// </summary>
        /// <response code="200">Returns the status</response>
        /// <response code="404">If the status does not exists</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StatusResponseDto>> GetStatusById(int id)
        {
            return await ReturnResult<ResultContainer<StatusResponseDto>, StatusResponseDto>
                (_statusService.GetStatusById(id));
        }
        
        /// <summary>
        /// Creates a status
        /// </summary>
        /// <param name="statusDto"></param>
        /// <response code="201">Returns the newly created status</response>
        /// <response code="204">If the status already exists</response>
        /// <response code="400">If the status is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StatusRequestDto>> CreateStatus(StatusRequestDto statusDto)
        {
            return await ReturnResult<ResultContainer<StatusResponseDto>, StatusResponseDto>
                (_statusService.CreateStatus(statusDto));
        }
        
        
        /// <summary>
        /// Delete the status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If the status was deleted successfully</response>
        /// <response code="404">If the status does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            return await ReturnResult<ResultContainer<StatusResponseDto>, StatusResponseDto>
                (_statusService.DeleteStatus(id));
        }
        
        /// <summary>
        /// Update the status
        /// </summary>
        /// <response code="204">If the status was updated successfully</response>
        /// <response code="400">If the status is null</response>
        /// <response code="404">If the status does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(StatusRequestDto statusDto)
        {
            return await ReturnResult<ResultContainer<StatusResponseDto>, StatusResponseDto>
                (_statusService.UpdateStatus(statusDto));
        }
    }
}