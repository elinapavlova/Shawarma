using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Role;


namespace Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApiContext _db;

        public RoleRepository(ApiContext context)
        {
            _db = context;
        }

        public async Task<ICollection<Role>> GetRoleList()
        {
            var roles = await _db.Roles.ToListAsync();
            return roles; 
        }
        
        public async void CreateRole(Role role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
        }

        public void UpdateRole(Role newRole)
        {
            var role = _db.Roles.FindAsync(newRole.Id).Result;
            
            role.Name = newRole.Name;
            
            _db.Roles.Update(role);
            _db.SaveChangesAsync();
        }

        public async void DeleteRole(int id)
        {
            var role = _db.Roles.FindAsync(id).Result;
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            return role ?? null; 
        }
    }
}