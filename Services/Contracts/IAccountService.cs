using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Order;
using Models.Shawarma;
using Models.User;
using Models.ViewModels;

namespace Services.Contracts
{
    public interface IAccountService
    {
        void CreateOrder(ResultContainer<UserResponseDto> user, string rows);
        Task<IndexViewModel<ShawarmaResponseDto>> GetShawarmaPage( bool needOnlyActual, int page = 1);
        Task<IndexViewModel<OrderResponseDto>> GetOrdersPage( bool needOnlyActual, int page = 1);
        Task<IndexViewModel<OrderResponseDto>> GetOrdersByUserPage(int id, int page = 1);
    }
}