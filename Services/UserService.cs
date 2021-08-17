using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
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
        
        public async Task<ICollection<UserResponseDto>> GetUserList()
        {
            var users = _mapper.Map<ICollection<UserResponseDto>>(await _repository.GetUserList());
            return users;
        }

        public async Task<UserResponseDto> GetUserById(int id)
        {
            var user = _mapper.Map<UserResponseDto>(await _repository.GetUserById(id));

            return user;
        }

        public void CreateUser(UserRequestDto userDto)
        {
            var roleOfUser = _roleService.GetRoleById(userDto.IdRole).Result;

            if (roleOfUser == null) 
                return;
            
            var user = _mapper.Map<User>(userDto);
            _repository.CreateUser(user);
        }

        public void UpdateUser(UserRequestDto userDto)
        {
            var roleOfUser = _roleService.GetRoleById(userDto.IdRole).Result;

            if (roleOfUser == null) 
                return;

            var user = _mapper.Map<User>(userDto);
            _repository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            _repository.DeleteUser(id);
        }
    }
}