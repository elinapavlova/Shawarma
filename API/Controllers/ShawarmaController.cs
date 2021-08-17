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
        
        /// <summary>
        /// Gets all shawarmas
        /// </summary>
        /// <response code="200">Returns all shawarmas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ICollection<ShawarmaResponseDto>> GetShawarmaList()
        {
            var shawarmas = _shawarmaService.GetShawarmaList().Result;
            return await Task.FromResult(shawarmas);
        }
        
        /// <summary>
        /// Gets shawarma by id
        /// </summary>
        /// <response code="200">Returns the shawarma</response>
        /// <response code="404">If the shawarma does not exists</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShawarmaResponseDto>> GetShawarmaById(int id)
        {
            var shawarma = _shawarmaService.GetShawarmaById(id).Result;
            
            if (shawarma == null) 
                return NotFound();
            
            return await Task.FromResult<ActionResult<ShawarmaResponseDto>>(shawarma);
        }
        
        /// <summary>
        /// Creates a shawarma
        /// </summary>
        /// <param name="shawarmaDto"></param>
        /// <response code="201">Returns the newly created shawarma</response>
        /// <response code="204">If the shawarma already exists</response>
        /// <response code="400">If the shawarma is null</response>
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
            return await Task.FromResult<ActionResult<ShawarmaRequestDto>>
                (CreatedAtAction("GetShawarmaById", new {id = shawarmaDto.Id}, shawarmaDto));
        }
        
        /// <summary>
        /// Delete the shawarma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If the shawarma was deleted successfully</response>
        /// <response code="404">If the shawarma does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShawarma(int id)
        {
            var shawarma = _shawarmaService.GetShawarmaById(id).Result;

            if (shawarma == null) 
                return NotFound();
            
            _shawarmaService.DeleteShawarma(id);
            return NoContent();
        }
        
        /// <summary>
        /// Update the shawarma
        /// </summary>
        /// <response code="204">If the shawarma was updated successfully</response>
        /// <response code="400">If the shawarma is null</response>
        /// <response code="404">If the shawarma does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            var shawarma = _shawarmaService.GetShawarmaById(shawarmaDto.Id).Result;
            
            if (shawarma == null) 
                return NotFound();
            
            _shawarmaService.UpdateShawarma(shawarmaDto);
            return NoContent();
        }
    }
}