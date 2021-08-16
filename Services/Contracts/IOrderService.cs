using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<ICollection<OrderResponseDto>> GetOrderList();
        Task<OrderResponseDto> GetOrderById(int id);
        void CreateOrder(OrderRequestDto order);
        void UpdateOrder(OrderRequestDto order);
        void DeleteOrder(int id);
    }
}