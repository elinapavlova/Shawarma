using System.Threading.Tasks;
using Infrastructure.Error;
using Infrastructure.Result;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        
        public AuthService
        (
            IUserService userService,
            IJwtService jwtService
        )
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        
        public async Task<ResultContainer<UserResponseDto>> VerifyUser(string username, string password)
        {
            var getUserByEmail = await _userService.GetUserByEmail(username);

            if (getUserByEmail.Data == null)
            {
                getUserByEmail.ErrorType = ErrorType.NotFound;
                return getUserByEmail;
            }
            
            if (username == getUserByEmail.Data.Email && password == getUserByEmail.Data.Password)
                return getUserByEmail;

            var res = new ResultContainer<UserResponseDto>
            {
                ErrorType = ErrorType.BadRequest
            };
            return res;
        }
        
        public async Task<ResultContainer<UserResponseDto>> LoginUser(UserLoginDto dto)
        {
            var user = await VerifyUser(dto.Email, dto.Password);
            return user;
        }
        
        public async Task<ResultContainer<UserResponseDto>> RegisterUser(UserRequestDto dto)
        {
            var user = await _userService.GetUserByEmail(dto.Email);

            if (user.Data != null)
            {
                user.ErrorType = ErrorType.BadRequest;
                return user;
            }

            var result = await _userService.CreateUser(dto);
            
            return result;
        }
    }
}