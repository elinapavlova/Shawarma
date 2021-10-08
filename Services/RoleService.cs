using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Error;
using Models.Role;
using Services.Contracts;

namespace Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService
        (
            IRoleRepository repository, 
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultContainer<ICollection<RoleResponseDto>>> GetListByPage(int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<RoleResponseDto>>>
                (await _repository.GetPage(pageSize, page));
            
            return result;
        }

        public async Task<ResultContainer<ICollection<RoleResponseDto>>> GetList()
        {
            var roles = _mapper.Map<ResultContainer<ICollection<RoleResponseDto>>>(await _repository.GetList());
            return roles;
        }

        public async Task<ResultContainer<RoleResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<RoleResponseDto>();
            var role = await _repository.GetById(id);
            
            if (role == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<RoleResponseDto>>(role);

            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> Create(RoleRequestDto roleDto)
        {
            var getRole = await GetById(roleDto.Id);

            if (getRole.Data != null)
            {
                getRole.ErrorType = ErrorType.BadRequest;
                return getRole;
            }

            var role = _mapper.Map<Role>(roleDto);
            var result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.Create(role));
            
            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> Edit(RoleRequestDto roleDto)
        {
            var getRole = await GetById(roleDto.Id);
            ResultContainer<RoleResponseDto> result;

            if (getRole.Data == null)
            {
                result = _mapper.Map<ResultContainer<RoleResponseDto>>(await Create(roleDto));
                return result;
            }
            
            var role = _mapper.Map<Role>(roleDto);
            result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.Edit(role));
            
            return result;
        }

        public async Task<ResultContainer<RoleResponseDto>> Delete(int id)
        {
            var getRole = await GetById(id);

            if (getRole.Data == null)
                return getRole;

            var result = _mapper.Map<ResultContainer<RoleResponseDto>>(await _repository.Delete(id));
            result.Data = null;
            
            return result;
        }
        
        public async Task<SelectList> GetSelectList()
        {
            var roles = await GetList();
            var rolesList = new SelectList(roles.Data, "Id", "Name");
            return rolesList;
        }
    }
}