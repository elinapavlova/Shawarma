using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Error;
using Infrastructure.Result;
using Models.Order;
using Services.Contracts;

namespace Services
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUserService _userService;
        private readonly IStatusService _statusService;
        private readonly IMapper _mapper;
        
        public OrderService(IOrderRepository repository, IMapper mapper, IUserService userService,
            IStatusService statusService)
        {
            _repository = repository;
            _userService = userService;
            _statusService = statusService;
            _mapper = mapper;
        }
        
        public async Task<ResultContainer<ICollection<OrderResponseDto>>> GetOrderList()
        {
            var orders = _mapper.Map<ResultContainer<ICollection<OrderResponseDto>>>(await _repository.GetOrderList());
            return orders;
        }

        public async Task<ResultContainer<OrderResponseDto>> GetOrderById(int id)
        {
            ResultContainer<OrderResponseDto> result = new ResultContainer<OrderResponseDto>();
            var getOrder = await _repository.GetOrderById(id);

            if (getOrder == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.GetOrderById(id));
            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> CreateOrder(OrderRequestDto orderDto)
        {
            //    if (orderDto.Date.Hour is > 12 or < 9) return null;
            var user = await _userService.GetUserById(orderDto.IdUser);
            var getOrder = await GetOrderById(orderDto.Id);

            if (getOrder.Data != null || user.Data == null)
            {
                getOrder.ErrorType = ErrorType.BadRequest;
                return getOrder;
            }

            var newOrder = _mapper.Map<Order>(orderDto);
            newOrder.IdStatus = 1;
            var result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.CreateOrder(newOrder));

            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> UpdateOrder(OrderRequestDto orderDto)
        {
            var user = await _userService.GetUserById(orderDto.IdUser);
            var status = await _statusService.GetStatusById(orderDto.IdStatus);
            var getOrder = await GetOrderById(orderDto.Id);
            ResultContainer<OrderResponseDto> result;

            if (status.Data == null || getOrder.Data == null)
            {
                getOrder.ErrorType = ErrorType.NotFound;
                return getOrder;
            }

            if (user.Data == null)
            {
                result = _mapper.Map<ResultContainer<OrderResponseDto>>(await CreateOrder(orderDto));
                return result;
            }

            var newOrder = _mapper.Map<Order>(orderDto);
            result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.UpdateOrder(newOrder));
            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> DeleteOrder(int id)
        {
            var getOrder = await GetOrderById(id);

            if (getOrder.Data == null)
                return getOrder;

            var result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.DeleteOrder(id));
            result.Data = null;

            return result;
        }
    }
}