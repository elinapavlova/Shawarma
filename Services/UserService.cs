using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Error;
using Infrastructure.Result;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper, IRoleService roleService)
        {
            _repository = repository;
            _mapper = mapper;
            _roleService = roleService;
        }
        
        public async Task<ResultContainer<ICollection<UserResponseDto>>> GetUserList()
        {
            var users = _mapper.Map<ResultContainer<ICollection<UserResponseDto>>>(await _repository.GetUserList());
            return users;
        }

        public async Task<ResultContainer<UserResponseDto>> GetUserById(int id)
        {
            ResultContainer<UserResponseDto> result = new ResultContainer<UserResponseDto>();
            
            var getUser = await _repository.GetUserById(id);
            
            if (getUser == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.GetUserById(id));

            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> CreateUser(UserRequestDto userDto)
        {
            var getUser = await GetUserById(userDto.Id);
            var roleOfUser = await _roleService.GetRoleById(userDto.IdRole);

            if (getUser.Data != null || roleOfUser.Data == null)
            {
                getUser.ErrorType = ErrorType.BadRequest;
                return getUser;
            }

            var user = _mapper.Map<User>(userDto);
            var result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.CreateUser(user));
            
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> UpdateUser(UserRequestDto userDto)
        {
            var getUser = await GetUserById(userDto.Id);
            var roleOfUser = await _roleService.GetRoleById(userDto.IdRole);

            if (getUser.Data == null || roleOfUser == null)
            {
                getUser.ErrorType = ErrorType.NotFound;
                return getUser;
            }
            
            var user = _mapper.Map<User>(userDto);
            var result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.UpdateUser(user));
            
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> DeleteUser(int id)
        {
            var getUser = await GetUserById(id);

            if (getUser.Data == null)
                return getUser;

            var result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.DeleteUser(id));
            result.Data = null;
            
            return result;
        }
    }
}