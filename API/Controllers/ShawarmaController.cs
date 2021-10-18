using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Shawarma;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class ShawarmaController : BaseController
    {
        private readonly IShawarmaService _shawarmaService;
        private readonly int _pageSize;
        public ShawarmaController
        (
            IShawarmaService service,
            AppSettingsOptions appSetings
        )
        {
            _shawarmaService = service;
            _pageSize = appSetings.DefaultPageSize;
        }

        /// <summary>
        /// Get actual shawarmas
        /// </summary>
        /// <response code="200">Returns actual shawarmas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultContainer<ICollection<ShawarmaResponseDto>>>> GetShawarmaList(int page)
        {
            return await ReturnResult<ResultContainer<ICollection<ShawarmaResponseDto>>,
                ICollection<ShawarmaResponseDto>>(_shawarmaService.GetListByPage(_pageSize, true, page));;
        }

        [HttpGet("Count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> Count(bool onlyActual)
        {
            return await ReturnResult<ResultContainer<int>, int>(_shawarmaService.Count(onlyActual));
        }

        /// <summary>
        /// Get shawarma by id
        /// </summary>
        /// <response code="200">Returns the shawarma</response>
        /// <response code="404">If the shawarma does not exists</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<ShawarmaResponseDto>>> GetShawarmaById(int id)
        {
            return await ReturnResult<ResultContainer<ShawarmaResponseDto>, ShawarmaResponseDto>
                (_shawarmaService.GetById(id));
        }
        
        /// <summary>
        /// Get shawarma by name
        /// </summary>
        /// <response code="200">Returns the shawarma</response>
        /// <response code="404">If the shawarma does not exists</response>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<ShawarmaResponseDto>>> GetShawarmaByName(string name)
        {
            return await ReturnResult<ResultContainer<ShawarmaResponseDto>, ShawarmaResponseDto>
                (_shawarmaService.GetByName(name));
        }
        
        /// <summary>
        /// Create a shawarma
        /// </summary>
        /// <param name="shawarmaDto"></param>
        /// <response code="201">Returns the newly created shawarma</response>
        /// <response code="204">If the shawarma already exists</response>
        /// <response code="400">If the shawarma is null</response>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ResultContainer<ShawarmaResponseDto>>> CreateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            return await ReturnResult<ResultContainer<ShawarmaResponseDto>, ShawarmaResponseDto>
                (_shawarmaService.Create(shawarmaDto));
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
        public async Task<ActionResult<ResultContainer<ShawarmaResponseDto>>> DeleteShawarma(int id)
        {
            return await ReturnResult<ResultContainer<ShawarmaResponseDto>, ShawarmaResponseDto>
                (_shawarmaService.Delete(id));
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
        public async Task<ActionResult<ResultContainer<ShawarmaResponseDto>>> UpdateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            return await ReturnResult<ResultContainer<ShawarmaResponseDto>, ShawarmaResponseDto>
                (_shawarmaService.Edit(shawarmaDto));
        }
    }
}