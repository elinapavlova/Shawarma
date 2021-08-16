using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly Order _order;

        public OrderController(IOrderService orderService, IStatusService statusService,
            IUserService userService)
        {
            _orderService = orderService;
            _statusService = statusService;
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<ICollection<OrderResponseDto>> GetOrderList()
        {
            var orders = _orderService.GetOrderList().Result;
            return orders;
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id).Result;

            if (order == null) 
                return NotFound();
            
            return order;
        }
        
        [HttpPost]
        public async Task<ActionResult<OrderRequestDto>> CreateOrder(OrderRequestDto orderDto)
        {
            var status = _statusService.GetStatusById(orderDto.IdStatus).Result;
            var user = _userService.GetUserById(orderDto.IdUser).Result;
            
            if (status == null || user == null) 
                return NotFound();
            
            var order = _orderService.GetOrderById(orderDto.Id).Result;

            if (order != null) 
                return NoContent();
            
            _orderService.CreateOrder(orderDto);
            return CreatedAtAction("GetOrderById", new {id = orderDto.Id}, orderDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = _orderService.GetOrderById(id).Result;

            if (order == null) 
                return NotFound();
            
            _orderService.DeleteOrder(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderRequestDto orderDto)
        {
            var order = await _orderService.GetOrderById(orderDto.Id);
            
            if (order == null) 
                return NotFound();
            
            _orderService.UpdateOrder(orderDto);
            return NoContent();
        }
    }
}