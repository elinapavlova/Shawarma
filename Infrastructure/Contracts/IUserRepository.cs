using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUserList();

        Task<User> CreateUser(User user);

        Task<User> UpdateUser(User user);
        
        Task<User> DeleteUser(int id);

        Task<User> GetUserById(int id);
    }
}