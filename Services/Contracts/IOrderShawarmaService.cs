using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.OrderShawarma;

namespace Services.Contracts
{
    public interface IOrderShawarmaService
    {
        Task<ResultContainer<ICollection<OrderShawarmaResponseDto>>> GetListByPage(int pageSize, int page = 1);
        Task<ResultContainer<OrderShawarmaResponseDto>> GetById(int id);
        Task<ResultContainer<OrderShawarmaResponseDto>> Create(OrderShawarmaRequestDto shawarma);
        Task<ResultContainer<OrderShawarmaResponseDto>> Edit(OrderShawarmaRequestDto shawarma);
        Task<ResultContainer<OrderShawarmaResponseDto>> Delete(int id);
    }
}