using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.OrderShawarma;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderShawarmaController : BaseController
    {
        private readonly IOrderShawarmaService _orderShawarmaService;
        private readonly int _pageSize;
        public OrderShawarmaController
        (
            IOrderShawarmaService orderShawarmaService,
            AppSettingsOptions appSettings
        )
        {
            _orderShawarmaService = orderShawarmaService;
            _pageSize = appSettings.DefaultPageSize;
        }
        
        /// <summary>
        /// Get all shawarmas from orders
        /// </summary>
        /// <response code="200">Returns all orders</response>
        [HttpGet]
        public async Task<ActionResult<ResultContainer<ICollection<OrderShawarmaResponseDto>>>> GetOrderShawarmaList(int page = 1)
        {
            return await ReturnResult<ResultContainer<ICollection<OrderShawarmaResponseDto>>, 
                    ICollection<OrderShawarmaResponseDto>>
                (_orderShawarmaService.GetListByPage(_pageSize, page));
        }
        
        /// <summary>
        /// Get shawarmas from order by id
        /// </summary>
        /// <response code="200">Returns shawarmas from order</response>
        /// <response code="404">If the order or type of shawarmas does not exists</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResultContainer<OrderShawarmaResponseDto>>> GetOrderShawarmaById(int id)
        {
            return await ReturnResult<ResultContainer<OrderShawarmaResponseDto>, OrderShawarmaResponseDto>
                (_orderShawarmaService.GetById(id));
        }
        
        /// <summary>
        /// Create a shawarma for order
        /// </summary>
        /// <param name="orderShawaDto"></param>
        /// <response code="201">Returns the newly created shawarma for order</response>
        /// <response code="204">If the type of shawarma is not actual</response>
        /// <response code="400">If the shawarmas for order is null or order does not exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ResultContainer<OrderShawarmaResponseDto>>> CreateOrderShawarma
            (OrderShawarmaRequestDto orderShawaDto)
        {
            return await ReturnResult<ResultContainer<OrderShawarmaResponseDto>, OrderShawarmaResponseDto>
                (_orderShawarmaService.Create(orderShawaDto));
        }
        
        /// <summary>
        /// Delete shawarmas from order
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">If shawarmas from order was deleted successfully</response>
        /// <response code="404">If shawarmas from order does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<OrderShawarmaResponseDto>>> DeleteOrderShawarma(int id)
        {
            return await ReturnResult<ResultContainer<OrderShawarmaResponseDto>, OrderShawarmaResponseDto>
                (_orderShawarmaService.Delete(id));
        }
        
        /// <summary>
        /// Update shawarmas from order
        /// </summary>
        /// <response code="204">If shawarmas from order was updated successfully</response>
        /// <response code="400">If the type of shawarma is not actual</response>
        /// <response code="404">If shawarmas from order does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<OrderShawarmaResponseDto>>> 
            UpdateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            return await ReturnResult<ResultContainer<OrderShawarmaResponseDto>, OrderShawarmaResponseDto>
                (_orderShawarmaService.Edit(orderShawaDto));
        }
    }
}