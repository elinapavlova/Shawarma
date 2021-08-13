using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Shawarma;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShawarmaController : ControllerBase
    {
        private readonly IShawarmaService _shawarmaService;

        public ShawarmaController(IShawarmaService service)
        {
            _shawarmaService = service;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ICollection<ShawarmaResponseDto>> GetShawarmaList()
        {
            var shawarmas = _shawarmaService.GetShawarmaList().Result;
            return shawarmas;
        }
        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShawarmaResponseDto>> GetShawarmaById(int id)
        {
            var shawarma = _shawarmaService.GetShawarmaById(id).Result;
            
            if (shawarma == null) 
                return NotFound();
            
            return shawarma;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ShawarmaRequestDto>> CreateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            var shawarma = _shawarmaService.GetShawarmaById(shawarmaDto.Id).Result;

            if (shawarma != null) 
                return NoContent();
            
            _shawarmaService.CreateShawarma(shawarmaDto);
            return CreatedAtAction("GetShawarmaById", new {id = shawarmaDto.Id}, shawarmaDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShawarma(int id)
        {
            var shawarma = _shawarmaService.GetShawarmaById(id).Result;

            if (shawarma == null) 
                return NotFound();
            
            _shawarmaService.DeleteShawarma(id);
            return NoContent();
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShawarma(int id, ShawarmaRequestDto shawarmaDto)
        {
            if (id != shawarmaDto.Id) 
                return BadRequest();

            var shawarma = await _shawarmaService.GetShawarmaById(id);
            
            if (shawarma == null) 
                return NotFound();
            
            _shawarmaService.UpdateShawarma(id, shawarmaDto);
            return NoContent();
        }
    }
}