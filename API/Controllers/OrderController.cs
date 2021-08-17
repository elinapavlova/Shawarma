using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Order;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IStatusService _statusService;

        public OrderController(IOrderService orderService, IStatusService statusService,
            IUserService userService)
        {
            _orderService = orderService;
            _statusService = statusService;
            _userService = userService;
        }
        
        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <response code="200">Returns all orders</response>
        [HttpGet]
        public async Task<ICollection<OrderResponseDto>> GetOrderList()
        {
            var orders = _orderService.GetOrderList().Result;
            return await Task.FromResult(orders);
        }
        
        /// <summary>
        /// Gets order by id
        /// </summary>
        /// <response code="200">Returns the order</response>
        /// <response code="404">If the order does not exists</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id).Result;

            if (order == null) 
                return NotFound();
            
            return await Task.FromResult<ActionResult<OrderResponseDto>>(order);
        }
        
        /// <summary>
        /// Creates a order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="204">If the order already exists</response>
        /// <response code="400">If the order is null or user or status does not exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<OrderRequestDto>> CreateOrder(OrderRequestDto orderDto)
        {
            var status = _statusService.GetStatusById(orderDto.IdStatus).Result;
            var user = _userService.GetUserById(orderDto.IdUser).Result;
            
            if (status == null || user == null) 
                return BadRequest();
            
            var order = _orderService.GetOrderById(orderDto.Id).Result;

            if (order != null) 
                return NoContent();
            
            _orderService.CreateOrder(orderDto);
            return await Task.FromResult<ActionResult<OrderRequestDto>>
                (CreatedAtAction("GetOrderById", new {id = orderDto.Id}, orderDto));
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
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = _orderService.GetOrderById(id).Result;

            if (order == null) 
                return NotFound();
            
            _orderService.DeleteOrder(id);
            return NoContent();
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
        public async Task<IActionResult> UpdateOrder(OrderRequestDto orderDto)
        {
            var status = _statusService.GetStatusById(orderDto.IdStatus).Result;
            var user = _userService.GetUserById(orderDto.IdUser).Result;
            var order = _orderService.GetOrderById(orderDto.Id).Result;

            if (order == null)
                return NotFound();

            if (status == null || user == null) 
                return BadRequest();
            
            _orderService.UpdateOrder(orderDto);
            return NoContent();
        }
    }
}