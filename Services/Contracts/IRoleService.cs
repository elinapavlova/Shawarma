using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Role;

namespace Services.Contracts
{
    public interface IRoleService
    {
        Task<ResultContainer<ICollection<RoleResponseDto>>> GetRoleList();
        Task<ResultContainer<RoleResponseDto>> GetRoleById(int id);
        Task<ResultContainer<RoleResponseDto>> CreateRole(RoleRequestDto role);
        Task<ResultContainer<RoleResponseDto>>UpdateRole(RoleRequestDto role);
        Task<ResultContainer<RoleResponseDto>> DeleteRole(int id);
    }
}