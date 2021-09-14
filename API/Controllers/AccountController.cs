using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Infrastructure.Result;
using Models.User;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly AuthController _authController;
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        
        public AccountController
        (
            IRoleService roleService,
            IAuthService authService,
            IAccountService accountService,
            IJwtService jwtService
        )
        {
            _roleService = roleService;
            _authService = authService;
            _jwtService = jwtService;
            _authController = new AuthController(authService, jwtService);
            _accountService = accountService;
        }
        
        public async Task<ActionResult> IndexAdmin()
        {
            var user = await _authController.VerifyJwt();
            
            if (user.ErrorType.HasValue)
                return Unauthorized();
            
            ViewBag.Id = user.Data.Id;
            return View();
        }

        public async Task<ActionResult> IndexClient()
        {
            var user = await _authController.VerifyJwt();

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
            
            await _authController.CreateAndSaveJwt(dto);

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
            var user = await _authController.VerifyJwt();

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
            var user = await _authController.VerifyJwt();
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
            var user = await _authController.VerifyJwt();
            var viewModel = await _accountService.GetPage(true, page);
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles =  await _roleService.GetSelectList();
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
        public async Task<IActionResult> Logout()
        {
            await _authController.Logout();
            return RedirectToAction("Login");
        }
    }
}