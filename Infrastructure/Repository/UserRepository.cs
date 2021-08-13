using System.Collections.Generic;
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

        public UserRepository(ApiContext context)
        {
            _db = context;
        }

        public async Task<ICollection<User>> GetUserList()
        {
            var users = await _db.Users.ToListAsync();
            return users; 
        }
        
        public async void CreateUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public void UpdateUser(long id, User newUser)
        {
            var user = _db.Users.FindAsync(id).Result;
            
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.UserName = newUser.UserName;
            user.IdRole = newUser.IdRole;
            
            _db.Users.Update(user);
            _db.SaveChangesAsync();
        }

        public async void DeleteUser(long id)
        {
            var user = _db.Users.FindAsync(id).Result;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetUserById(long id)
        {
            var user = await _db.Users.FindAsync(id);
            return user; 
        }
    }
}