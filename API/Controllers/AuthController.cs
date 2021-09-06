using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Models.User;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController
        (
            IAuthService authService
        )
        {
            _authService = authService;
        }
        
       
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRequestDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.RegisterUser(dto));
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.LoginUser(dto));
        }
    }
}