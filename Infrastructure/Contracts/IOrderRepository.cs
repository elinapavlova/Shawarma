using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;

namespace Infrastructure.Contracts
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetOrderList();

        void CreateOrder(Order order);
        
        void UpdateOrder(Order order);
        
        void DeleteOrder(int id);

        Task<Order> GetOrderById(int id);
    }
}