using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Models.Role;
using Services.Contracts;

namespace Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<ICollection<RoleResponseDto>> GetRoleList()
        {
            var roles = _mapper.Map<ICollection<RoleResponseDto>>(await _repository.GetRoleList());
            return roles;
        }

        public async Task<RoleResponseDto> GetRoleById(int id)
        {
            var role = _mapper.Map<RoleResponseDto>(await _repository.GetRoleById(id));
            return role;
        }

        public void CreateRole(RoleRequestDto roleDto)
        {
            if (roleDto.Name == null) 
                return;
            var role = _mapper.Map<Role>(roleDto);
            _repository.CreateRole(role);
        }

        public void UpdateRole(int id, RoleRequestDto roleDto)
        {
            if (roleDto.Name == null) 
                return;
            
            var role = _mapper.Map<Role>(roleDto);
            _repository.UpdateRole(id, role);
        }

        public void DeleteRole(int id)
        {
            _repository.DeleteRole(id);
        }
    }
}