using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Role;

namespace Services.Contracts
{
    public interface IRoleService
    {
        Task<ICollection<RoleResponseDto>> GetRoleList();
        Task<RoleResponseDto> GetRoleById(int id);
        void CreateRole(RoleRequestDto role);
        void UpdateRole(int id, RoleRequestDto role);
        void DeleteRole(int id);
    }
}