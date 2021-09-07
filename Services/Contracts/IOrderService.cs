using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Order;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<ResultContainer<ICollection<OrderResponseDto>>> GetOrderList();
        Task<ResultContainer<ICollection<OrderResponseDto>>> GetActualOrderList(DateTime date);
        Task<ResultContainer<OrderResponseDto>> GetOrderById(int id);
        Task<ResultContainer<OrderResponseDto>> CreateOrder(OrderRequestDto order);
        Task<ResultContainer<OrderResponseDto>> UpdateOrder(OrderRequestDto order);
        Task<ResultContainer<OrderResponseDto>> DeleteOrder(int id);
    }
}