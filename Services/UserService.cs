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
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper, IRoleRepository roleRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        
        public async Task<ICollection<UserResponseDto>> GetUserList()
        {
            var users = _mapper.Map<ICollection<UserResponseDto>>(await _repository.GetUserList());
            return users;
        }

        public async Task<UserResponseDto> GetUserById(long id)
        {
            var user = _mapper.Map<UserResponseDto>(await _repository.GetUserById(id));
            return user;
        }

        public void CreateUser(UserRequestDto userDto)
        {
            var role = _roleRepository.GetRoleById(userDto.IdRole);

            if (userDto.Email == null || userDto.UserName == null || userDto.Password == null || role == null)  
                return;
            var user = _mapper.Map<User>(userDto);
            _repository.CreateUser(user);
        }

        public void UpdateUser(long id, UserRequestDto userDto)
        {
            if (userDto.Email != null && userDto.UserName != null && userDto.Password != null)
            {
                var user = _mapper.Map<User>(userDto);
                 _repository.UpdateUser(id, user);               
            }
        }

        public void DeleteUser(long id)
        {
            _repository.DeleteUser(id);
        }
    }
}