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

        public RoleRepository()
        {
            _db = new ApiContext();
        }

        public async Task<ICollection<Role>> GetRoleList()
        {
            var roles = await _db.Roles.ToListAsync();
            return roles; 
        }
        
        public async Task<Role> CreateRole(Role role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRole(Role newRole)
        {
            var role = await _db.Roles.FindAsync(newRole.Id);
            
            role.Name = newRole.Name;
            
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> DeleteRole(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> GetRoleById(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            return role; 
        }
    }
}