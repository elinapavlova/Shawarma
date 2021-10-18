using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Tokens;
using Services.Contracts;
using Models.User;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserRequestDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Register(dto));
        }
        
        /// <summary>
        /// Authorization
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> Login(UserCredentialsDto dto)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>, AccessTokenDto>(_authService.Login(dto));
        }
        
        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>,AccessTokenDto>
                (_authService.RefreshTokenAsync(refreshToken.RefreshToken, refreshToken.EmailUser));
        }
    }
}