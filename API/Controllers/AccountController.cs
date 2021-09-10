using System;
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
        private readonly AuthController _authController;
        private readonly IJwtService _jwtService;
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController
        (
            IRoleService roleService,
            IJwtService jwtService,
            IAuthService authService,
            IAccountService accountService
        )
        {
            _roleService = roleService;
            _jwtService = jwtService;
            _authService = authService;
            _authController = new AuthController(_authService);
            _accountService = accountService;
        }
        
        public async Task<ActionResult> IndexAdmin()
        {
            var user = await VerifyUser();
            
            if (user.ErrorType.HasValue)
                return Unauthorized();
            
            ViewBag.Id = user.Data.Id;
            return View();
        }

        public async Task<ActionResult> IndexClient()
        {
            var user = await VerifyUser();

            if (user.ErrorType.HasValue)
                return Unauthorized();
            
            ViewBag.Id = user.Data.Id;
            return View();
        }

        public async Task<ActionResult<ResultContainer<UserResponseDto>>> Index(UserLoginDto dto)
        {
            var user = await _authService.VerifyUser(dto.Email, dto.Password);
            if (user.ErrorType.HasValue)
                return user;
            
            CreateAndSaveJwt(user);

            return user.Data.IdRole switch
            {
                1 => RedirectToAction("IndexAdmin", "Account"),
                2 => RedirectToAction("IndexClient", "Account"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        /// <summary>
        /// Переход к авторизации после успешной регистрации
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<object> RegisterToLogin(UserRequestDto dto, int role)
        {
            dto.IdRole = role;
            var result = await _authController.Register(dto);
            var ok = new OkObjectResult(result);

            return result.GetType() != ok.GetType() ? ok.Value : RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDataForOrder(string rows)
        {
            var length = rows.Length;
            var user = await VerifyUser();

            if (length < 3 || user.ErrorType.HasValue)
                return Json(false);

            _accountService.CreateOrder(user, rows);

            return Json(true);
        }
        
        /// <summary>
        /// Список шаурмы для администратора
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetShawarmaList(int page = 1)
        {
            var user = await VerifyUser();
            var viewModel = await _accountService.GetPage(false, page);
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        /// <summary>
        /// Список шаурмы для клиента
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetShawarmasForClient(int page = 1)
        {
            var user = await VerifyUser();
            var viewModel = await _accountService.GetPage(true, page);
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles =  await _roleService.GetRolesSelectList();
            return View();
        }
        
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Верификация пользователя по токену
        /// </summary>
        /// <returns></returns>
        private async Task<ResultContainer<UserResponseDto>> VerifyUser()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _authService.VerifyUserJwt(jwt);
            return user;
        }
        
        /// <summary>
        /// Создание и сохранение в сессии токена после успешной аутентификации
        /// </summary>
        /// <param name="user"></param>
        private void CreateAndSaveJwt(ResultContainer<UserResponseDto> user)
        {
            var jwt = _jwtService.Generate(user.Data.Id);
            
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });
        }
    }
}