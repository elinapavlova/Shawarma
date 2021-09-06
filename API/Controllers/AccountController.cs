using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.User;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IShawarmaService _shawarmaService;
        private readonly AuthController _authController;
        private readonly IJwtService _jwtService;
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController
        (
            IRoleService roleService,
            IShawarmaService shawarmaService,
            IJwtService jwtService,
            IAuthService authService,
            IAccountService accountService
        )
        {
            _roleService = roleService;
            _shawarmaService = shawarmaService;
            _jwtService = jwtService;
            _authService = authService;
            _authController = new AuthController(_authService);
            _accountService = accountService;
        }
        
        public async Task<ActionResult> IndexAdmin()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _accountService.VerifyUserJwt(jwt);
            
            return !user.ErrorType.HasValue ? View() : Unauthorized();
        }


        public async Task<ActionResult> IndexClient()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _accountService.VerifyUserJwt(jwt);
            
            return !user.ErrorType.HasValue ? View() : Unauthorized();
        }

        public async Task<ActionResult<ResultContainer<UserResponseDto>>> Index(UserLoginDto dto)
        {
            var user = await _authService.VerifyUser(dto.Email, dto.Password);

            if (user.ErrorType.HasValue)
                return user;
            
            var jwt = _jwtService.Generate(user.Data.Id);
            
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return user.Data.IdRole switch
            {
                1 => RedirectToAction("IndexAdmin", "Account"),
                2 => RedirectToAction("IndexClient", "Account"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public async Task<object> RegisterToLogin(UserRequestDto dto, int role)
        {
            dto.IdRole = role;
            var result = await _authController.Register(dto);
            var ok = new OkObjectResult(result);

            return result.GetType() != ok.GetType() ? ok.Value : RedirectToAction("Login", "Account");
        }

        public async Task<JsonResult> GetData(string rows)
        {
            var length = rows.Length;
            var jwt = Request.Cookies["jwt"];
            
            var user = await _accountService.VerifyUserJwt(jwt);

            if (length < 3)
                return Json(false);

            _accountService.CreateOrder(user, rows);

            return Json(true);
        }

        public async Task<IActionResult> GetShawarmaList()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _accountService.VerifyUserJwt(jwt);

            var shawarmas = await _shawarmaService.GetShawarmaList();
            var orderDtos = shawarmas.Data.ToList();

            return !user.ErrorType.HasValue ? View(orderDtos) : Unauthorized();
        }
        
        public async Task<IActionResult> GetShawarmasForClient()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _accountService.VerifyUserJwt(jwt);

            var shawarmas = await _shawarmaService.GetActualShawarmaList();
            var orderDtos = shawarmas.Data.ToList();
            
            return !user.ErrorType.HasValue ? View(orderDtos) : Unauthorized();
        }
        
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles =  await _roleService.GetRolesSelectList();
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }
    }
}