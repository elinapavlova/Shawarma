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
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IShawarmaService _shawarmaService;
        private readonly IStatusService _statusService;
        private readonly IExportActualOrdersToExcelService _exportService;
        
        public AccountController
        (
            IRoleService roleService,
            IAuthService authService,
            IAccountService accountService,
            IJwtService jwtService,
            IShawarmaService shawarmaService,
            IStatusService statusService,
            IExportActualOrdersToExcelService exportService
        )
        {
            _roleService = roleService;
            _authService = authService;
            _jwtService = jwtService;
            _accountService = accountService;
            _shawarmaService = shawarmaService;
            _statusService = statusService;
            _exportService = exportService;
        }
        
        public async Task<ActionResult> IndexAdmin()
        {
            var user = await VerifyJwt();
            ViewBag.Id = user.Data.Id;
            
            if (user.ErrorType.HasValue)
                return Unauthorized();
            
            return View();
        }

        public async Task<ActionResult> IndexClient()
        {
            var user = await VerifyJwt();
            ViewBag.Id = user.Data.Id;

            if (user.ErrorType.HasValue)
                return Unauthorized();
            
            return View();
        }

        public async Task<ActionResult<ResultContainer<UserResponseDto>>> Index(UserLoginDto dto)
        {
            var user = await _authService.VerifyUser(dto.Email, dto.Password);
            
            if (user.ErrorType.HasValue)
                return user;
            
            await CreateAndSaveJwt(dto);
            ViewBag.Id = user.Data.Id;
            
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
            var result = await _authService.Register(dto);

            return result.ErrorType.HasValue ? result : RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDataForOrder(string rows)
        {
            var length = rows.Length;
            var user = await VerifyJwt();

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
            var user = await VerifyJwt();
            var viewModel = await _accountService.GetShawarmaPage(false, page);
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        public async Task<IActionResult> GetOrderList(int page = 1)
        {
            var user = await VerifyJwt();
            var viewModel = await _accountService.GetOrdersPage(false, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = await _statusService.GetList();

            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses.Data;
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        public async Task<IActionResult> GetActualOrderList(int page = 1)
        {
            var user = await VerifyJwt();
            var viewModel = await _accountService.GetOrdersPage(true, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = await _statusService.GetList();

            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses.Data;
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        /// <summary>
        /// Список шаурмы для клиента
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetShawarmasForClient(int page = 1)
        {
            var user = await VerifyJwt();
            var viewModel = await _accountService.GetShawarmaPage(true, page);
            
            return !user.ErrorType.HasValue ? View(viewModel) : Unauthorized();
        }
        
        /// <summary>
        /// Список заказов клиента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrdersOfClient(int id, int page = 1)
        {
            var user = await VerifyJwt();
            var isAuthorize = await IsEqualIdAndJwtId(id);

            if (!isAuthorize) 
                return Unauthorized();
            
            var viewModel = await _accountService.GetOrdersByUserPage(id, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = await _statusService.GetList();
            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses.Data;

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
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }
        
        /// <summary>
        /// Выбор excel-страницы для выгрузки шаурмы в базу данных
        /// </summary>
        /// <returns></returns>
        public IActionResult ImportFromExcel()
        {
            return View();
        }

        /// <summary>
        /// Верификация Jwt-токена
        /// </summary>
        /// <returns></returns>
        private async Task<ResultContainer<UserResponseDto>> VerifyJwt()
        {
            var jwt = Request.Cookies["jwt"];
            var user = await _authService.VerifyJwt(jwt);

            return user;
        }

        /// <summary>
        /// Проверка Id пользователя и Id пользователя из Jwt-токена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> IsEqualIdAndJwtId(int id)
        {
            var idFromJwt = await VerifyJwt();
            return id == idFromJwt.Data.Id;
        }
        
        /// <summary>
        /// Создание и сохранение Jwt-токена в куки
        /// </summary>
        /// <param name="dto"></param>
        private async Task CreateAndSaveJwt(UserLoginDto dto)
        {
            var user = await _authService.VerifyUser(dto.Email, dto.Password);

            if (!user.ErrorType.HasValue)
            {
                var jwt = _jwtService.Generate(user.Data.Id, user.Data.IdRole);
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true
                });                
            }
        }
    }
}