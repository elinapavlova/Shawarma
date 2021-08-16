using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Status;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService service)
        {
            _statusService = service;
        }
        
        [HttpGet]
        public async Task<ICollection<StatusResponseDto>> GetStatusList()
        {
            var statuses = _statusService.GetStatusList().Result;
            return statuses;
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StatusResponseDto>> GetStatusById(int id)
        {
            var status = _statusService.GetStatusById(id).Result;
            
            if (status == null) 
                return NotFound();
            
            return status;
        }
        
        [HttpPost]
        public async Task<ActionResult<StatusRequestDto>> CreateStatus(StatusRequestDto statusDto)
        {
            var status = _statusService.GetStatusById(statusDto.Id).Result;

            if (status != null) 
                return NoContent();
            
            _statusService.CreateStatus(statusDto);
            return CreatedAtAction("GetStatusById", new {id = statusDto.Id}, statusDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = _statusService.GetStatusById(id).Result;

            if (status == null) 
                return NotFound();
            
            _statusService.DeleteStatus(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateStatus(StatusRequestDto statusDto)
        {
            var status = await _statusService.GetStatusById(statusDto.Id);
            
            if (status == null) 
                return NotFound();
            
            _statusService.UpdateStatus(statusDto);
            return NoContent();
        }
    }
}