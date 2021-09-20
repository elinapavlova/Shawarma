using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Infrastructure.Validate;
using Models.Error;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService
        (
            IUserRepository repository, 
            IMapper mapper, 
            IRoleService roleService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _roleService = roleService;
        }
        
        public async Task<ResultContainer<ICollection<UserResponseDto>>> GetList()
        {
            var result = _mapper.Map<ResultContainer<ICollection<UserResponseDto>>>
                (await _repository.GetList());
            return result;
        }

        public async Task<ResultContainer<ICollection<UserResponseDto>>> GetListByPage(int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<UserResponseDto>>>
                (await _repository.GetPage(pageSize, page));
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<UserResponseDto>();
            var getUser = await _repository.GetById(id);
            
            if (getUser == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<UserResponseDto>>(getUser);
            return result;
        }
        
        public async Task<ResultContainer<UserResponseDto>> GetByEmail(string email)
        {
            var result = new ResultContainer<UserResponseDto>();
            var getUser = await _repository.GetUserByEmail(email);
            
            if (getUser == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<UserResponseDto>>(getUser);
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> Create(UserRequestDto userDto)
        {
            var getUser = await GetByEmail(userDto.Email);
            var roleOfUser = await _roleService.GetById(userDto.IdRole);
            
            if (userDto.Email == null)
            {
                getUser.ErrorType = ErrorType.BadRequest;
                return getUser;
            }

            if (getUser.Data != null || roleOfUser.Data == null)
            {
                getUser.ErrorType = ErrorType.BadRequest;
                return getUser;
            }
            
            var user = _mapper.Map<User>(userDto);
            var result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.Create(user));
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> Edit(UserRequestDto userDto)
        {
            var getUser = await GetById(userDto.Id);
            var roleOfUser = await _roleService.GetById(userDto.IdRole);
            var isValidEmail = Validator.EmailIsValid(userDto.Email);

            if (roleOfUser.Data == null)
            {
                getUser.ErrorType = ErrorType.NotFound;
                return getUser;
            }
            
            if (!isValidEmail)
            {
                getUser.ErrorType = ErrorType.BadRequest;
                return getUser;
            }

            var user = _mapper.Map<User>(userDto);
            ResultContainer<UserResponseDto> result;
            
            if (getUser.Data == null)
            {
                result = _mapper.Map<ResultContainer<UserResponseDto>>(await Create(userDto));
                return result;
            }
            
            result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.Edit(user));
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> Delete(int id)
        {
            var getUser = await GetById(id);

            if (getUser.Data == null)
                return getUser;

            var result = _mapper.Map<ResultContainer<UserResponseDto>>(await _repository.Delete(id));
            result.Data = null;
            return result;
        }
    }
}