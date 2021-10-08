using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.User;

namespace Infrastructure.Repository
{
    public sealed class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApiContext _db;

        public UserRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
            _db = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email != null && email == u.Email);
            return user;
        }

        public new async Task<User> GetById(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null) 
                return null;
            
            var orders = await GetOrders(id);
            user.Orders = orders;

            return user;
        }

        private async Task<List<Order>> GetOrders(int id)
        {
            var orders = await _db.Orders
                .Where(o => o.IdUser == id)
                .ToListAsync();

            foreach (var order in orders)
            {
                var orderShawarmas = await _db.OrderShawarmas
                    .Where(os => os.OrderId == order.Id)
                    .ToListAsync();
                
                order.OrderShawarmas = orderShawarmas;
            }
            
            return orders;
        }
    }
}