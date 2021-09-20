using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;
using Models.User;

namespace Infrastructure.Contracts
{
    public interface IUserRepository : IBaseRepository<User>

    {
        Task<User> GetUserByEmail(string email);
        Task<List<Order>> GetOrders(int id);
    }
}