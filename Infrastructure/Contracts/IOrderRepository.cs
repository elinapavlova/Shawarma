using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetOrderList();

        void Create(Order order);
        
        void Update(Order order);
        
        void Delete(Order order);

        void GetStatusById(long id);
    }
}