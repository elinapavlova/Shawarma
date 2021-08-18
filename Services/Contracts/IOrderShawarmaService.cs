using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.OrderShawarma;

namespace Services.Contracts
{
    public interface IOrderShawarmaService
    {
        Task<ResultContainer<ICollection<OrderShawarmaResponseDto>>> GetOrderShawarmaList();
        Task<ResultContainer<OrderShawarmaResponseDto>> GetOrderShawarmaById(int id);
        Task<ResultContainer<OrderShawarmaResponseDto>> CreateOrderShawarma(OrderShawarmaRequestDto shawarma);
        Task<ResultContainer<OrderShawarmaResponseDto>> UpdateOrderShawarma(OrderShawarmaRequestDto shawarma);
        Task<ResultContainer<OrderShawarmaResponseDto>> DeleteOrderShawarma(int id);
    }
}