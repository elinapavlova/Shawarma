using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Shawarma;
using Models.User;
using Models.ViewModels;

namespace Services.Contracts
{
    public interface IAccountService
    {
        void CreateOrder(ResultContainer<UserResponseDto> user, string rows);
        Task<IndexViewModel<ShawarmaResponseDto>> GetPage( bool needOnlyActual, int page = 1);
    }
}