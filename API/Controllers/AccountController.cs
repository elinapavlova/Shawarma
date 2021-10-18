using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Models.Enums;
using Models.User;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly IShawarmaService _shawarmaService;

        public AccountController
        (
            IAuthService authService,
            IAccountService accountService,
            IShawarmaService shawarmaService
        )
        {
            _authService = authService;
            _accountService = accountService;
            _shawarmaService = shawarmaService;
        }
        
        public async Task<ActionResult> IndexAdmin()
        {
            return View();
        }
        
        public async Task<ActionResult> IndexClient()
        {
            return View();
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
            //_accountService.CreateOrder(user, rows);

            return Json(true);
        }
        
        /// <summary>
        /// Список шаурмы для администратора
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetShawarmaList(int page = 1)
        {
            var viewModel = await _accountService.GetShawarmaPage(false, page);
            
            return View(viewModel);
        }
        
        public async Task<IActionResult> GetOrderList(int page = 1)
        {
            var viewModel = await _accountService.GetOrdersPage(false, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = Enum.GetValues(typeof(RolesEnum));

            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses;
            
            return View(viewModel);
        }
        
        public async Task<IActionResult> GetActualOrderList(int page = 1)
        {
            var viewModel = await _accountService.GetOrdersPage(true, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = Enum.GetValues(typeof(RolesEnum));

            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses;
            
            return View(viewModel);
        }
        
        /// <summary>
        /// Список шаурмы для клиента
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetShawarmasForClient(int page = 1)
        {
            var viewModel = await _accountService.GetShawarmaPage(true, page);
            
            return View(viewModel);
        }
        
        /// <summary>
        /// Список заказов клиента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetOrdersOfClient(int id, int page = 1)
        {
            var viewModel = await _accountService.GetOrdersByUserPage(id, page);
            var shawarmas = await _shawarmaService.GetList();
            var statuses = Enum.GetValues(typeof(RolesEnum));
            ViewBag.Shawarmas = shawarmas.Data;
            ViewBag.Statuses = statuses;

            return  View(viewModel);

        }
        
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles =  Enum.GetNames(typeof(RolesEnum));
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
        /// Выбор excel-страницы для выгрузки шаурмы в базу данных
        /// </summary>
        /// <returns></returns>
        public IActionResult ImportFromExcel()
        {
            return View();
        }
    }
}