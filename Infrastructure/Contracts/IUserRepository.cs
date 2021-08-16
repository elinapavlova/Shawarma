using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUserList();

        void CreateUser(User user);

        void UpdateUser(User user);
        
        void DeleteUser(int id);

        Task<User> GetUserById(int id);
    }
}