using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OrderShawarma;

namespace Infrastructure.Contracts
{
    public interface IOrderShawarmaRepository
    {
        Task<ICollection<OrderShawarma>> GetOrderShawarmaList();

        Task<OrderShawarma> CreateOrderShawarma(OrderShawarma orderShawarma);
        
        Task<OrderShawarma> UpdateOrderShawarma(OrderShawarma orderShawarma);
        
        Task<OrderShawarma> DeleteOrderShawarma(int id);

        Task<OrderShawarma> GetOrderShawarmaById(int id);
    }
}