using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.User;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiContext _db;
        private readonly IOrderRepository _orderRepository;

        public UserRepository(ApiContext context, IOrderRepository orderRepository)
        {
            _db = context;
            _orderRepository = orderRepository;
        }

        public async Task<ICollection<User>> GetUserList()
        {
            var users = await _db.Users.ToListAsync();
            
            var orders = await _orderRepository.GetOrderList();
            
            if (orders.Count == 0)
                return users;
            
            foreach (var user in users)
            {
                var ordersOfUser = orders.Where(o => o.IdUser == user.Id).ToList();
                user.Orders = ordersOfUser;
            }
            
            return users; 
        }
        
        public async Task<User> CreateUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUser(User newUser)
        {
            var user = await _db.Users.FindAsync(newUser.Id);
            
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.UserName = newUser.UserName;
            user.IdRole = newUser.IdRole;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) 
                return null;
            
            var orders = await _orderRepository.GetOrderList();
            if (orders.Count == 0)
                return user;
            
            var ordersOfUser = orders.Where(o => o.IdUser == user.Id).ToList();

            user.Orders = ordersOfUser;
            return user;
        }
    }
}