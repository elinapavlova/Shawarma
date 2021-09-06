using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IAccountService
    {
        Task<ResultContainer<UserResponseDto>> VerifyUserJwt(string jwt);
        void CreateOrder(ResultContainer<UserResponseDto> user, string rows);
    }
}