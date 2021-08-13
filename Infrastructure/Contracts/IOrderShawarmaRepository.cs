using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OrderShawarma;

namespace Infrastructure.Contracts
{
    public interface IOrderShawarmaRepository
    {
        Task<ICollection<OrderShawarma>> GetOrderShawarmaList();

        void Create(OrderShawarma orderShawarma);
        
        void Update(OrderShawarma orderShawarma);
        
        void Delete(OrderShawarma orderShawarma);

        void GetOrderShawarmaById(long Orderid);
    }
}