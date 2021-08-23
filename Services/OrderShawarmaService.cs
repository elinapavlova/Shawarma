using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Error;
using Infrastructure.Result;
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

        public async Task<ResultContainer<ICollection<OrderShawarmaResponseDto>>> GetOrderShawarmaList()
        {
            var orderShawas = _mapper.Map<ResultContainer<ICollection<OrderShawarmaResponseDto>>>
                (await _repository.GetOrderShawarmaList());
            
            return orderShawas;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> GetOrderShawarmaById(int id)
        {
            ResultContainer<OrderShawarmaResponseDto> result = new ResultContainer<OrderShawarmaResponseDto>();
            
            var getOrderShawarma = await _repository.GetOrderShawarmaById(id);
            
            if (getOrderShawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await _repository.GetOrderShawarmaById(id));

            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> CreateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var getOrderShawa = await GetOrderShawarmaById(orderShawaDto.Id);
            var order =await _orderService.GetOrderById(orderShawaDto.OrderId);
            var shawa = await _shawarmaService.GetShawarmaById(orderShawaDto.ShawarmaId);

            if (order.Data == null || shawa.Data is not {IsActual: true})
            {
                getOrderShawa.ErrorType = ErrorType.BadRequest;
                return getOrderShawa;
            }
            
            var orderShawas = await GetOrderShawarmaList();
            
            foreach (var os in orderShawas.Data)
            {
                // Если уже существует заказ с таким видом шаурмы, увеличить их количество
                            
                if (os.OrderId != orderShawaDto.OrderId || os.ShawarmaId != orderShawaDto.ShawarmaId) continue;
                            
                os.Number += orderShawaDto.Number;
            
                var newOrderShawa = _mapper.Map<OrderShawarmaRequestDto>(os);
                
                var getResult = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                    (await UpdateOrderShawarma(newOrderShawa));
                
                return getResult;
            }
            
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            var result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                (await _repository.CreateOrderShawarma(orderShawa));
            
            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> UpdateOrderShawarma(OrderShawarmaRequestDto orderShawaDto)
        {
            var getOrderShawa = await GetOrderShawarmaById(orderShawaDto.Id);
            var order =await _orderService.GetOrderById(orderShawaDto.OrderId);
            var shawa = await _shawarmaService.GetShawarmaById(orderShawaDto.ShawarmaId);
            ResultContainer<OrderShawarmaResponseDto> result;
                
            if (order.Data == null || shawa.Data is not {IsActual: true})
            {
                getOrderShawa.ErrorType = ErrorType.BadRequest;
                return getOrderShawa;
            }

            if (getOrderShawa.Data == null)
            {
                result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                    (await CreateOrderShawarma(orderShawaDto));
                
                return result;
            }
            
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                (await _repository.UpdateOrderShawarma(orderShawa));
            
            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> DeleteOrderShawarma(int id)
        {
            var getOrderShawa = await GetOrderShawarmaById(id);

            if (getOrderShawa.Data == null)
                return getOrderShawa;

            var result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await _repository.DeleteOrderShawarma(id));
            result.Data = null;
            
            return result;
        }
    }
}