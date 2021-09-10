using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<ResultContainer<UserResponseDto>> VerifyUser(string username, string password);
        Task<ResultContainer<UserResponseDto>> VerifyUserJwt(string jwt);
        Task<ResultContainer<UserResponseDto>> LoginUser(UserLoginDto dto);
        Task<ResultContainer<UserResponseDto>> RegisterUser(UserRequestDto dto);
    }
}