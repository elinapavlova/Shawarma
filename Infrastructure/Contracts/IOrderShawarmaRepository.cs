using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OrderShawarma;

namespace Infrastructure.Contracts
{
    public interface IOrderShawarmaRepository
    {
        Task<ICollection<OrderShawarma>> GetOrderShawarmaList();

        void CreateOrderShawarma(OrderShawarma orderShawarma);
        
        void UpdateOrderShawarma(OrderShawarma orderShawarma);
        
        void DeleteOrderShawarma(int id);

        Task<OrderShawarma> GetOrderShawarmaById(int id);
    }
}