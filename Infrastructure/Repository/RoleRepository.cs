using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Role;


namespace Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApiContext _db;
        private readonly int _appSettingsConfiguration;
        
        public RoleRepository
        (
            ApiContext context,
            IConfiguration configuration
        )
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<ICollection<Role>> ApplyPaging(ICollection<Role> source, int pageSize, int page = 1)
        {
            var roles = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return roles;
        }

        public async Task<ICollection<Role>> GetList()
        {
            var roles = await _db.Roles.OrderBy(r => r.Id).ToListAsync();
            return roles; 
        }

        public async Task<int> Count()
        {
            var count = await _db.Roles.CountAsync();
            return count;
        }

        public async Task<ICollection<Role>> GetPage(int pageSize, int page = 1)
        {
            var source = await GetList();
            var result = await ApplyPaging(source, _appSettingsConfiguration, page);
            return result;
        }

        public async Task<Role> Create(Role role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> Edit(Role newRole)
        {
            var role = await _db.Roles.FindAsync(newRole.Id);
            
            role.Name = newRole.Name;
            
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> Delete(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return role;
        }

        public async Task<Role> GetById(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            return role; 
        }
    }
}