using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.User;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiContext _db;

        public UserRepository(ApiContext context)
        {
            _db = context;
        }

        public async Task<ICollection<User>> GetUserList()
        {
            var users = await _db.Users.ToListAsync();
            List<Order> orders = null;
            foreach (var user in users)
            {
                orders = await _db.Orders.Where(o => o.IdUser == user.Id).ToListAsync();
                
                if (orders != null) 
                    foreach (var order in orders) 
                    { 
                        var orderShawas
                         = await _db.OrderShawarmas.Where(o => o.OrderId == order.Id).ToListAsync(); 
                        order.OrderShawarmas = orderShawas; 
                    }
                
                user.Orders = orders;
            }
            
            return users; 
        }
        
        public async void CreateUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public void UpdateUser(User newUser)
        {
            var user = _db.Users.FindAsync(newUser.Id).Result;
            
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.UserName = newUser.UserName;
            user.IdRole = newUser.IdRole;

            _db.Users.Update(user);
            _db.SaveChangesAsync();
        }

        public async void DeleteUser(int id)
        {
            var user = _db.Users.FindAsync(id).Result;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) 
                return null;
            
            var orders = await _db.Orders.ToListAsync();
            if (orders.Count == 0)
                return user;
            
            var ordersOfUser = await _db.Orders.Where(o => o.IdUser == user.Id).ToListAsync();
            if (ordersOfUser.Count == 0)
                return user;
            
            foreach (var order in orders)
            {
                var orderShawas
                    = await _db.OrderShawarmas.Where(o => o.OrderId == order.Id).ToListAsync();
                order.OrderShawarmas = orderShawas;
            }
            
            user.Orders = ordersOfUser;
            return user;
        }
    }
}