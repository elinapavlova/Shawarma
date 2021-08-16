using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Role;

namespace Infrastructure.Contracts
{
    public interface IRoleRepository
    {
        Task<ICollection<Role>> GetRoleList();

        void CreateRole(Role role);
        
        void UpdateRole(Role role);
        
        void DeleteRole(int id);

        Task<Role> GetRoleById(int id);
    }
}