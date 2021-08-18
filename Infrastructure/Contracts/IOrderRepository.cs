using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;

namespace Infrastructure.Contracts
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetOrderList();

        Task<Order> CreateOrder(Order order);
        
        Task<Order> UpdateOrder(Order order);
        
        Task<Order> DeleteOrder(int id);

        Task<Order> GetOrderById(int id);
    }
}