using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OrderShawarma;

namespace Infrastructure.Contracts
{
    public interface IOrderShawarmaRepository : IBaseRepository<OrderShawarma>
    {
    }
}