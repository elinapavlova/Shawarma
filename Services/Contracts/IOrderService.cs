using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Order;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<ResultContainer<ICollection<OrderResponseDto>>> GetActualListByPage
            (DateTime date, int pageSize, int page = 1);
        Task<int> Count();
        Task<ResultContainer<ICollection<OrderResponseDto>>> GetListByPage(int pageSize, int page = 1);
        Task<ResultContainer<ICollection<OrderResponseDto>>> GetActualList(DateTime date);
        Task<ResultContainer<OrderResponseDto>> GetById(int id);
        Task<ResultContainer<OrderResponseDto>> Create(OrderRequestDto order);
        Task<ResultContainer<OrderResponseDto>> Edit(OrderRequestDto order);
        Task<ResultContainer<OrderResponseDto>> Delete(int id);
    }
}