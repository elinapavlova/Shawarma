using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Result;
using Models.Role;

namespace Services.Contracts
{
    public interface IRoleService
    {
        Task<ResultContainer<ICollection<RoleResponseDto>>> GetListByPage(int pageSize, int page = 1);
        Task<ResultContainer<ICollection<RoleResponseDto>>> GetList();
        Task<ResultContainer<RoleResponseDto>> GetById(int id);
        Task<SelectList> GetSelectList();
        Task<ResultContainer<RoleResponseDto>> Create(RoleRequestDto role);
        Task<ResultContainer<RoleResponseDto>>Edit(RoleRequestDto role);
        Task<ResultContainer<RoleResponseDto>> Delete(int id);
    }
}