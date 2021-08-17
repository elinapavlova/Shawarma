using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Models.OrderShawarma;
using Services.Contracts;

namespace Services
{
    public class OrderShawarmaService : IOrderShawarmaService
    {
        private readonly IOrderShawarmaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IShawarmaService _shawarmaService;

        public OrderShawarmaService(IOrderShawarmaRepository repository, IMapper mapper, IOrderService orderService,
            IShawarmaService shawarmaService)
        {
            _repository = repository;
            _mapper = mapper;
            _orderService = orderService;
            _shawarmaService = shawarmaService;
        }

        public async Task<ICollection<OrderShawarmaResponseDto>> GetOrderShawarmaList()
        {
            var orderShawas = 
                _mapper.Map<ICollection<OrderShawarmaResponseDto>>(await _repository.GetOrderShawarmaList());
            
            return orderShawas;
        }

        public async Task<OrderShawarmaResponseDto> GetOrderShawarmaById(int id)
        {
            var orderShawa = _mapper.Map<OrderShawarmaResponseDto>(await _repository.GetOrderShawarmaById(id));
            
            return orderShawa;
        }

        public void CreateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            var orderShawas = GetOrderShawarmaList().Result;

            foreach (var os in orderShawas)
            {
                // Если уже существует заказ с таким видом шаурмы, увеличить их количество
                
                if (os.OrderId != orderShawaDto.OrderId || os.ShawarmaId != orderShawaDto.ShawarmaId) continue;
                
                os.Number += orderShawaDto.Number;

                var newOrderShawa = _mapper.Map<OrderShawarmaRequestDto>(os);
                UpdateOrderShawarma(newOrderShawa);
                return;
            }
            _repository.CreateOrderShawarma(orderShawa);
        }

        public void UpdateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var order = _orderService.GetOrderById(orderShawaDto.OrderId).Result;
            var shawa = _shawarmaService.GetShawarmaById(orderShawaDto.ShawarmaId).Result;

            if (order == null || shawa== null)  
                return;
            
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            _repository.UpdateOrderShawarma(orderShawa);
        }

        public void DeleteOrderShawarma(int id)
        {
             _repository.DeleteOrderShawarma(id);
        }
    }
}