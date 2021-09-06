using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Models.Account;
using Models.Shawarma;
using Models.User;
using Services;
using Services.Contracts;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly UserController _userController;
        private readonly BaseController _base;
        private readonly IShawarmaService _shawarmaService;
        private readonly ShawarmaController _controller;
        private readonly IOrderService _orderService;
        private readonly IJwtService _jwtService;

        public HomeController(IUserService service, IRoleService roleService, 
            IShawarmaService shawarmaService, IOrderService orderService, IJwtService jwtService)
        {
            _roleService = roleService;
            if (service != null)
                _userController = new UserController(service);
            _userService = service;
            _base = new BaseController();
            _shawarmaService = shawarmaService;
            _controller = new ShawarmaController(_shawarmaService);
            _orderService = orderService;
            _jwtService = jwtService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
    }
}