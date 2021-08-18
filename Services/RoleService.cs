using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Error;
using Infrastructure.Result;
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
        
        public async Task<ResultContainer<ICollection<RoleResponseDto>>> GetRoleList()
        {
            var roles = _mapper.Map<ResultContainer<ICollection<RoleResponseDto>>>(await _repository.GetRoleList());
            return roles;
        }

        public async Task<ResultContainer<RoleResponseDto>> GetRoleById(int id)
        {
            ResultContainer<RoleResponseDto> result = new ResultContainer<RoleResponseDto>();
            
            var getRole = await _repository.GetRoleById(id);
            
            if (getRole == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.GetRoleById(id));

            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> CreateRole(RoleRequestDto roleDto)
        {
            var getRole = await GetRoleById(roleDto.Id);

            if (getRole.Data != null)
            {
                getRole.ErrorType = ErrorType.BadRequest;
                return getRole;
            }

            var role = _mapper.Map<Role>(roleDto);
            var result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.CreateRole(role));
            
            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> UpdateRole(RoleRequestDto roleDto)
        {
            var getRole = await GetRoleById(roleDto.Id);

            if (getRole.Data == null)
                return getRole;

            var role = _mapper.Map<Role>(roleDto);
            var result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.UpdateRole(role));
            
            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> DeleteRole(int id)
        {
            var getRole = await GetRoleById(id);

            if (getRole.Data == null)
                return getRole;

            var result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.DeleteRole(id));
            result.Data = null;
            
            return result;
        }
    }
}