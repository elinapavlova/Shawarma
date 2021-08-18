using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ResultContainer<ICollection<UserResponseDto>>> GetUserList();
        Task<ResultContainer<UserResponseDto>> GetUserById(int id);
        Task<ResultContainer<UserResponseDto>> CreateUser(UserRequestDto user);
        Task<ResultContainer<UserResponseDto>> UpdateUser(UserRequestDto user);
        Task<ResultContainer<UserResponseDto>> DeleteUser(int id);
    }
}