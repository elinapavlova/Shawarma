using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Models.User;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
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
        
       /// <summary>
       /// Registration
       /// </summary>
       /// <param name="dto"></param>
       /// <returns></returns>
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRequestDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Register(dto));
        }
        
        /// <summary>
        /// Authorisation
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(dto));
        }
    }
}