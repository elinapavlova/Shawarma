using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.OrderShawarma;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderShawarmaController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IShawarmaService _shawarmaService;
        private readonly IOrderShawarmaService _orderShawarmaService;

        public OrderShawarmaController(IOrderService orderService, IShawarmaService shawarmaService,
            IOrderShawarmaService orderShawarmaService)
        {
            _orderService = orderService;
            _shawarmaService = shawarmaService;
            _orderShawarmaService = orderShawarmaService;
        }
        
        [HttpGet]
        public async Task<ICollection<OrderShawarmaResponseDto>> GetOrderShawarmaList()
        {
            var orderShawas = _orderShawarmaService.GetOrderShawarmaList().Result;
            return orderShawas;
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderShawarmaResponseDto>> GetOrderShawarmaById(int id)
        {
            var orderShawa = _orderShawarmaService.GetOrderShawarmaById(id).Result;

            return orderShawa == null ? NotFound() : orderShawa;
        }
        
        [HttpPost]
        public async Task<ActionResult<OrderShawarmaRequestDto>> CreateOrderShawarma
            (OrderShawarmaRequestDto orderShawaDto)
        {
            var order = _orderService.GetOrderById(orderShawaDto.OrderId).Result;
            var shawarma = _shawarmaService.GetShawarmaById(orderShawaDto.ShawarmaId).Result;

            if (order == null || shawarma == null) 
                return NotFound();

            if (!shawarma.IsActual)
                return NoContent();

            _orderShawarmaService.CreateOrderShawarma(orderShawaDto);
            return CreatedAtAction("GetOrderShawarmaById", new {id = orderShawaDto.Id}, orderShawaDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrderShawarma(int id)
        {
            var orderShawa = _orderShawarmaService.GetOrderShawarmaById(id).Result;

            if (orderShawa == null) 
                return NotFound();
            
            _orderShawarmaService.DeleteOrderShawarma(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var orderShawa = await _orderShawarmaService.GetOrderShawarmaById(orderShawaDto.Id);
            
            if (orderShawa == null) 
                return NotFound();
            
            _orderShawarmaService.UpdateOrderShawarma(orderShawaDto);
            return NoContent();
        }
    }
}