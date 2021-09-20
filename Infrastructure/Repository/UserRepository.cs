using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.User;

namespace Infrastructure.Repository
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApiContext _db;
        private readonly int _appSettingsConfiguration;
        
        public UserRepository
        (
            ApiContext context, 
            IConfiguration configuration
        )
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email != null && email == u.Email);
            return user;
        }

        public async Task<ICollection<User>> GetPage(int pageSize, int page = 1)
        {
            var source = await GetList();
            var result = await ApplyPaging(source, _appSettingsConfiguration, page);
            return result;
        }

        public async Task<ICollection<User>> ApplyPaging(ICollection<User> source, int pageSize, int page = 1)
        {
            var shawarmas = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return shawarmas;
        }
        
        public async Task<ICollection<User>> GetList()
        {
            var users = await _db.Users
                .OrderBy(o => o.Id)
                .ToListAsync();
            return users; 
        }

        public async Task<int> Count()
        {
            var count = await _db.Users.CountAsync();
            return count;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null) 
                return null;
            
            var orders = await GetOrders(id);
            user.Orders = orders;

            return user;
        }

        public async Task<List<Order>> GetOrders(int id)
        {
            var orders = await _db.Orders
                .Where(o => o.IdUser == id)
                .ToListAsync();

            foreach (var order in orders)
            {
                var orde = await _db.OrderShawarmas
                    .Where(os => os.OrderId == order.Id)
                    .ToListAsync();
                
                order.OrderShawarmas = orde;
            }
            
            return orders;
        }

        public async Task<User> Create(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> Edit(User newUser)
        {
            var user = await _db.Users.FindAsync(newUser.Id);
            
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.UserName = newUser.UserName;
            user.IdRole = newUser.IdRole;
            user.Address = newUser.Address;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}