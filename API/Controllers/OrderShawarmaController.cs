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
        
        /// <summary>
        /// Gets all shawarmas from orders
        /// </summary>
        /// <response code="200">Returns all orders</response>
        [HttpGet]
        public async Task<ICollection<OrderShawarmaResponseDto>> GetOrderShawarmaList()
        {
            var orderShawas = _orderShawarmaService.GetOrderShawarmaList().Result;
            return await Task.FromResult(orderShawas);
        }
        
        /// <summary>
        /// Gets shawarmas from order by id
        /// </summary>
        /// <response code="200">Returns shawarmas from order</response>
        /// <response code="404">If the order or type of shawarmas does not exists</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderShawarmaResponseDto>> GetOrderShawarmaById(int id)
        {
            var orderShawa = _orderShawarmaService.GetOrderShawarmaById(id).Result;

            return orderShawa == null ? NotFound() : await Task.FromResult(orderShawa);
        }
        
        /// <summary>
        /// Creates a shawarma for order
        /// </summary>
        /// <param name="orderShawaDto"></param>
        /// <response code="201">Returns the newly created shawarma for order</response>
        /// <response code="204">If the type of shawarma is not actual</response>
        /// <response code="400">If the shawarmas for order is null or order does not exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<OrderShawarmaRequestDto>> CreateOrderShawarma
            (OrderShawarmaRequestDto orderShawaDto)
        {
            var order = _orderService.GetOrderById(orderShawaDto.OrderId).Result;
            var shawarma = _shawarmaService.GetShawarmaById(orderShawaDto.ShawarmaId).Result;

            if (order == null || shawarma == null) 
                return BadRequest();

            if (!shawarma.IsActual)
                return NoContent();

            _orderShawarmaService.CreateOrderShawarma(orderShawaDto);
            return await Task.FromResult<ActionResult<OrderShawarmaRequestDto>>
                (CreatedAtAction("GetOrderShawarmaById", new {id = orderShawaDto.Id}, orderShawaDto));
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
        public async Task<IActionResult> DeleteOrderShawarma(int id)
        {
            var orderShawa = _orderShawarmaService.GetOrderShawarmaById(id).Result;

            if (orderShawa == null) 
                return NotFound();
            
            _orderShawarmaService.DeleteOrderShawarma(id);
            return NoContent();
        }
        
        /// <summary>
        /// Update shawarmas from order
        /// </summary>
        /// <response code="204">If shawarmas from order was updated successfully</response>
        /// <response code="404">If shawarmas from order does not exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var orderShawa = _orderShawarmaService.GetOrderShawarmaById(orderShawaDto.Id).Result;
            
            if (orderShawa == null) 
                return NotFound();
            
            _orderShawarmaService.UpdateOrderShawarma(orderShawaDto);
            return NoContent();
        }
    }
}