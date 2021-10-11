using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ResultContainer<UserResponseDto>> GetById(int id);
        Task<ResultContainer<UserResponseDto>> GetByEmail(string email);
        Task<ResultContainer<UserResponseDto>> Create(UserRequestDto user);
        Task<ResultContainer<UserResponseDto>> Edit(UserRequestDto user);
    }
}