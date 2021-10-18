using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Tokens;
using Models.User;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<ResultContainer<AccessTokenDto>> Login(UserCredentialsDto dto);
        Task<ResultContainer<UserResponseDto>> Register(UserRequestDto dto);
        Task<ResultContainer<AccessTokenDto>> RefreshTokenAsync(string refreshToken, string userEmail);
    }
}