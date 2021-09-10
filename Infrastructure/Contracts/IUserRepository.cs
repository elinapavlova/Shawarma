using System.Collections.Generic;
using System.Threading.Tasks;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IUserRepository : IBaseRepository<User>

    {
        Task<User> GetUserByEmail(string email);
        Task<ICollection<User>> GetPage(int pageSize, int page = 1);
    }
}