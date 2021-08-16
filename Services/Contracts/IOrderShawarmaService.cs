using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OrderShawarma;

namespace Services.Contracts
{
    public interface IOrderShawarmaService
    {
        Task<ICollection<OrderShawarmaResponseDto>> GetOrderShawarmaList();
        Task<OrderShawarmaResponseDto> GetOrderShawarmaById(int id);
        void CreateOrderShawarma(OrderShawarmaRequestDto shawarma);
        void UpdateOrderShawarma(OrderShawarmaRequestDto shawarma);
        void DeleteOrderShawarma(int id);
    }
}