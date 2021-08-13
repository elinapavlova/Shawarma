using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ICollection<UserResponseDto>> GetUserList();
        Task<UserResponseDto> GetUserById(long id);
        void CreateUser(UserRequestDto user);
        void UpdateUser(long id, UserRequestDto user);
        void DeleteUser(long id);
    }
}