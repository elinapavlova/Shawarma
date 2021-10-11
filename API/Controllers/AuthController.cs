using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> Login(UserCredentialsDto dto)
        {
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>(Authenticate(dto));
        }
        
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Route("/api/logout")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        private async Task<ResultContainer<UserResponseDto>> Authenticate(UserCredentialsDto dto)
        {
            var user = await _authService.Login(dto);
            if (user.ErrorType.HasValue)
                return user;
            var principal = await _authService.CreatePrincipals(user.Data);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return user;
        }
    }
}