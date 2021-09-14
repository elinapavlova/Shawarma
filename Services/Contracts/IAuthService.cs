using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<ResultContainer<UserResponseDto>> VerifyUser(string username, string password);
        Task<ResultContainer<UserResponseDto>> VerifyJwt(string jwt);
        Task<ResultContainer<UserResponseDto>> Login(UserLoginDto dto);
        Task<ResultContainer<UserResponseDto>> Register(UserRequestDto dto);
    }
}