using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Models.User;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        public AuthController
        (
            IAuthService authService,
            IJwtService jwtService
        )
        {
            _authService = authService;
            _jwtService = jwtService;
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
            await CreateAndSaveJwt(dto);
            return await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(dto));
        }
        
        /// <summary>
        /// Logout
        /// </summary>
        [HttpPost("Logout")]
        public async Task Logout()
        {
            await DeleteJwt();
        }
        
        /// <summary>
        /// Create and save Jwt token in cookies
        /// </summary>
        /// <param name="dto"></param>
        internal async Task CreateAndSaveJwt(UserLoginDto dto)
        {
            var user = await _authService.VerifyUser(dto.Email, dto.Password);

            if (!user.ErrorType.HasValue)
            {
                var jwt = _jwtService.Generate(user.Data.Id);
                
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true
                });                
            }
        }

        /// <summary>
        /// Delete Jwt token from cookies
        /// </summary>
        private async Task DeleteJwt()
        {
            Response.Cookies.Delete("jwt");
        }
        
        /// <summary>
        /// Verification of user by token
        /// </summary>
        /// <returns></returns>
        internal async Task<ResultContainer<UserResponseDto>> VerifyJwt()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _authService.VerifyJwt(jwt);
            return user;
        }
    }
}