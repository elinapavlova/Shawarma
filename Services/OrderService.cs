using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
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
        
        public async Task<ICollection<OrderResponseDto>> GetOrderList()
        {
            var orders = _mapper.Map<ICollection<OrderResponseDto>>(await _repository.GetOrderList());
            return orders;
        }

        public async Task<OrderResponseDto> GetOrderById(int id)
        {
            var order = _mapper.Map<OrderResponseDto>(await _repository.GetOrderById(id));
            return order;
        }

        public void CreateOrder(OrderRequestDto orderDto)
        {
            //    if (orderDto.Date.Hour is > 12 or < 9) return;
            
            var order = _mapper.Map<Order>(orderDto);
            _repository.CreateOrder(order);
        }

        public void UpdateOrder(OrderRequestDto orderDto)
        {
            var user = _userService.GetUserById(orderDto.IdUser).Result;
            var status = _statusService.GetStatusById(orderDto.IdStatus).Result;

            if (user == null || status == null)  
                return;
            
            var order = _mapper.Map<Order>(orderDto);
            _repository.UpdateOrder(order);
        }

        public void DeleteOrder(int id)
        {
            var order = GetOrderById(id).Result;

            if (order.OrderShawarmas != null) 
                return;
            _repository.DeleteOrder(id);
        }
    }
}