using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models.Error;
using Models.Order;
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

        public OrderShawarmaService
        (
            IOrderShawarmaRepository repository, 
            IMapper mapper, 
            IOrderService orderService,
            IShawarmaService shawarmaService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _orderService = orderService;
            _shawarmaService = shawarmaService;
        }

        private async Task<ResultContainer<ICollection<OrderShawarmaResponseDto>>> GetList()
        {
            var orderShawas = _mapper.Map<ResultContainer<ICollection<OrderShawarmaResponseDto>>>
                (await _repository.GetList());
            
            return orderShawas;
        }

        public async Task<ResultContainer<ICollection<OrderShawarmaResponseDto>>> GetListByPage(int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<OrderShawarmaResponseDto>>>
                (await _repository.GetPage(pageSize, page));
            
            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<OrderShawarmaResponseDto>();
            var getOrderShawarma = await _repository.GetById(id);
            
            if (getOrderShawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await _repository.GetById(id));

            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> Create(OrderShawarmaRequestDto orderShawaDto)
        {
            var getOrderShawa = await GetById(orderShawaDto.Id);
            var order =await _orderService.GetById(orderShawaDto.OrderId);
            var shawa = await _shawarmaService.GetById(orderShawaDto.ShawarmaId);

            if (order.Data == null || shawa.Data is not {IsActual: true})
            {
                getOrderShawa.ErrorType = ErrorType.BadRequest;
                return getOrderShawa;
            }
            
            var orderShawas = await GetList();
            
            foreach (var os in orderShawas.Data)
            {
                // Если уже существует заказ с таким видом шаурмы, увеличить их количество
                if (os.OrderId != orderShawaDto.OrderId || os.ShawarmaId != orderShawaDto.ShawarmaId) 
                    continue;
                            
                os.Number += orderShawaDto.Number;
            
                var newOrderShawa = _mapper.Map<OrderShawarmaRequestDto>(os);
                var getResult = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await Edit(newOrderShawa));
                
                return getResult;
            }
            
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            var result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await _repository.Create(orderShawa));

            // Увеличение стоимости заказа
            order.Data.Cost += orderShawa.Number * shawa.Data.Cost;
            var editOrder = _mapper.Map<OrderResponseDto, OrderRequestDto>(order.Data);
            await _orderService.Edit(editOrder);
            
            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> Edit(OrderShawarmaRequestDto orderShawaDto)
        {
            var getOrderShawa = await GetById(orderShawaDto.Id);
            var order =await _orderService.GetById(orderShawaDto.OrderId);
            var shawa = await _shawarmaService.GetById(orderShawaDto.ShawarmaId);
            ResultContainer<OrderShawarmaResponseDto> result;
                
            if (order.Data == null || shawa.Data is not {IsActual: true})
            {
                getOrderShawa.ErrorType = ErrorType.BadRequest;
                return getOrderShawa;
            }

            if (getOrderShawa.Data == null)
            {
                result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                    (await Create(orderShawaDto));
                
                return result;
            }
            
            var orderShawa = _mapper.Map<OrderShawarma>(orderShawaDto);
            result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>
                (await _repository.Edit(orderShawa));
            
            return result;
        }

        public async Task<ResultContainer<OrderShawarmaResponseDto>> Delete(int id)
        {
            var getOrderShawa = await GetById(id);

            if (getOrderShawa.Data == null)
                return getOrderShawa;

            var result = _mapper.Map<ResultContainer<OrderShawarmaResponseDto>>(await _repository.Delete(id));
            result.Data = null;
            
            return result;
        }
    }
}