using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ICollection<UserResponseDto>> GetUserList();
        Task<UserResponseDto> GetUserById(int id);
        void CreateUser(UserRequestDto user);
        void UpdateUser(UserRequestDto user);
        void DeleteUser(int id);
    }
}