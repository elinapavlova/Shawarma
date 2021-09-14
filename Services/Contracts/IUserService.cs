using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ResultContainer<ICollection<UserResponseDto>>> GetList();
        Task<ResultContainer<ICollection<UserResponseDto>>> GetListByPage(int pageSize, int page = 1);
        Task<ResultContainer<UserResponseDto>> GetById(int id);
        Task<ResultContainer<UserResponseDto>> GetByEmail(string email);
        Task<ResultContainer<UserResponseDto>> Create(UserRequestDto user);
        Task<ResultContainer<UserResponseDto>> Edit(UserRequestDto user);
        Task<ResultContainer<UserResponseDto>> Delete(int id);
    }
}