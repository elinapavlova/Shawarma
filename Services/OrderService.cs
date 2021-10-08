using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models.Error;
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

        public OrderService
        (
            IOrderRepository repository, 
            IMapper mapper, 
            IUserService userService,
            IStatusService statusService
        )
        {
            _repository = repository;
            _userService = userService;
            _statusService = statusService;
            _mapper = mapper;
        }

        public async Task<ResultContainer<ICollection<OrderResponseDto>>> GetActualListByPage
            (DateTime date, int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<OrderResponseDto>>>
                (await _repository.GetActualListByPage(date, pageSize, page));
            
            return result;
        }

        public async Task<int> Count()
        {
            var count = await _repository.Count();
            return count;
        }

        public async Task<ResultContainer<ICollection<OrderDto>>> GetListByPage(int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<OrderDto>>>
                (await _repository.GetPage(pageSize, page));
            
            return result;
        }

        public async Task<ResultContainer<ICollection<OrderResponseDto>>> GetActualList(DateTime date)
        {
            var orders = _mapper.Map<ResultContainer<ICollection<OrderResponseDto>>>
                (await _repository.GetActualList(date));
            return orders;
        }
        
        public async Task<ResultContainer<OrderResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<OrderResponseDto>();
            var getOrder = await _repository.GetByIdWithShawarmas(id);

            if (getOrder == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<OrderResponseDto>>(getOrder);
            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> Create(OrderRequestDto orderDto)
        { 
            var result = new ResultContainer<OrderResponseDto>();
            
            /*
            if (orderDto.Date.Hour is > 12 or < 9)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            */
            
            var user = await _userService.GetById(orderDto.IdUser);
            var getOrder = await GetById(orderDto.Id);

            if (getOrder.Data != null || user.Data == null)
            {
                getOrder.ErrorType = ErrorType.BadRequest;
                return getOrder;
            }

            var newOrder = _mapper.Map<Order>(orderDto);
            result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.Create(newOrder));

            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> Edit(OrderRequestDto orderDto)
        {
            var user = await _userService.GetById(orderDto.IdUser);
            var status = await _statusService.GetById(orderDto.IdStatus);
            var getOrder = await GetById(orderDto.Id);
            ResultContainer<OrderResponseDto> result;

            if (status.Data == null || getOrder.Data == null)
            {
                getOrder.ErrorType = ErrorType.NotFound;
                return getOrder;
            }

            if (user.Data == null)
            {
                result = _mapper.Map<ResultContainer<OrderResponseDto>>(await Create(orderDto));
                return result;
            }

            var newOrder = _mapper.Map<Order>(orderDto);
            result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.Edit(newOrder));
            return result;
        }

        public async Task<ResultContainer<OrderResponseDto>> Delete(int id)
        {
            var getOrder = await GetById(id);

            if (getOrder.Data == null)
                return getOrder;

            var result = _mapper.Map<ResultContainer<OrderResponseDto>>(await _repository.Delete(id));
            result.Data = null;

            return result;
        }
    }
}