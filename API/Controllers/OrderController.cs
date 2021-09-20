using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.Order;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly int _pageSize;
        public OrderController
        (
            IOrderService orderService,
            IConfiguration configuration
        )
        {
            _orderService = orderService;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <response code="200">Returns all orders</response>
        [HttpGet]
        public async Task<ActionResult<ResultContainer<ICollection<OrderResponseDto>>>> GetOrderList(int page = 1)
        {
            return await ReturnResult<ResultContainer<ICollection<OrderResponseDto>>, ICollection<OrderResponseDto>>
                (_orderService.GetListByPage(_pageSize, page));
        }
        
        /// <summary>
        /// Get all actual (today) orders
        /// </summary>
        /// <response code="200">Returns all orders</response>
        [HttpGet("{date:datetime}")]
        public async Task<ActionResult<ResultContainer<ICollection<OrderResponseDto>>>> GetActualOrderList
            (DateTime date, int page = 1)
        {
            return await ReturnResult<ResultContainer<ICollection<OrderResponseDto>>, ICollection<OrderResponseDto>>
                (_orderService.GetActualListByPage(DateTime.Today, _pageSize, page));
        }
        
        /// <summary>
        /// Get order by id
        /// </summary>
        /// <response code="200">Returns the order</response>
        /// <response code="404">If the order does not exists</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            return await ReturnResult<ResultContainer<OrderResponseDto>, OrderResponseDto>
                (_orderService.GetById(id));
        }
        
        /// <summary>
        /// Create a order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="204">If the order already exists</response>
        /// <response code="400">If the order is null or user or status does not exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderRequestDto orderDto)
        {
            return await ReturnResult<ResultContainer<OrderResponseDto>, OrderResponseDto>
                ( _orderService.Create(orderDto));
        }
        
        /// <summary>
        /// Delete the order
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">If the order was deleted successfully</response>
        /// <response code="404">If the order does not exists</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> DeleteOrder(int id)
        {
            return await ReturnResult<ResultContainer<OrderResponseDto>, OrderResponseDto>
                (_orderService.Delete(id));
        }
        
        /// <summary>
        /// Update the order
        /// </summary>
        /// <response code="204">If the order was updated successfully</response>
        /// <response code="400">If the order is null or user or status is not exists</response>
        /// <response code="404">If the order does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrder(OrderRequestDto orderDto)
        {
            return await ReturnResult<ResultContainer<OrderResponseDto>, OrderResponseDto>
                (_orderService.Edit(orderDto));
        }
    }
}