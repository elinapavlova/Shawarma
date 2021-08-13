using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUserList();

        void CreateUser(User user);

        void UpdateUser(long id, User user);
        
        void DeleteUser(long id);

        Task<User> GetUserById(long id);
    }
}