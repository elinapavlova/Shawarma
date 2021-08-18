using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Role;

namespace Infrastructure.Contracts
{
    public interface IRoleRepository
    {
        Task<ICollection<Role>> GetRoleList();

        Task<Role> CreateRole(Role role);
        
        Task<Role> UpdateRole(Role role);
        
        Task<Role> DeleteRole(int id);

        Task<Role> GetRoleById(int id);
    }
}