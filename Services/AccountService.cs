using System;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure.Error;
using Infrastructure.Result;
using Models.Account;
using Models.Order;
using Models.OrderShawarma;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IShawarmaService _shawarmaService;
        private readonly IOrderShawarmaService _orderShawarmaService;
        
        public AccountService
        (
            IJwtService jwtService,
            IUserService userService,
            IOrderService orderService,
            IShawarmaService shawarmaService,
            IOrderShawarmaService orderShawarmaService
        )
        {
            _jwtService = jwtService;
            _userService = userService;
            _orderService = orderService;
            _shawarmaService = shawarmaService;
            _orderShawarmaService = orderShawarmaService;
        }
        
        public async Task<ResultContainer<UserResponseDto>> VerifyUserJwt(string jwt)
        {
            var user = new ResultContainer<UserResponseDto>();

            if (jwt == null)
            {
                user.ErrorType = ErrorType.Unauthorized;
                return user;
            }
            
            var token = _jwtService.Verify(jwt);
            
            if (token == null)
            {
                user.ErrorType = ErrorType.Unauthorized;
                return user;
            }
            
            var userId = int.Parse(token.Issuer);
            
            user = await _userService.GetUserById(userId);

            return user;
        }
        
        public async void CreateOrder(ResultContainer<UserResponseDto> user, string rows)
        {
            var orderFromJson = JsonSerializer.Deserialize<CreateOrderViewModel[]>(rows);
            
            var newOrder = new OrderRequestDto()
            {
                IdStatus = 1,
                IdUser = user.Data.Id
            };
            
            var createdOrder = await _orderService.CreateOrder(newOrder);

            var i = 0;
            
            while (orderFromJson != null && i < orderFromJson.Length)
            { 
                var shawarma = await _shawarmaService.GetShawarmaByName(orderFromJson[i].Name);

                var orderShawa = new OrderShawarmaRequestDto
                {
                    OrderId = createdOrder.Data.Id,
                    ShawarmaId = shawarma.Data.Id,
                    Number = Convert.ToInt32(orderFromJson[i].Number)
                };
                
                await _orderShawarmaService.CreateOrderShawarma(orderShawa);
                i++;
            }
        }
    }
}