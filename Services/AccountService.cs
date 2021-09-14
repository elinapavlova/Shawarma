using System;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.Extensions.Configuration;
using Models.Account;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.User;
using Models.ViewModels;
using Services.Contracts;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IOrderService _orderService;
        private readonly IShawarmaService _shawarmaService;
        private readonly IOrderShawarmaService _orderShawarmaService;
        private readonly int _appSettingsConfiguration;
        
        public AccountService
        (
            IOrderService orderService,
            IShawarmaService shawarmaService,
            IOrderShawarmaService orderShawarmaService,
            IConfiguration configuration
        )
        {
            _orderService = orderService;
            _shawarmaService = shawarmaService;
            _orderShawarmaService = orderShawarmaService;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async void CreateOrder(ResultContainer<UserResponseDto> user, string rows)
        {
            var orderFromJson = JsonSerializer.Deserialize<CreateOrderViewModel[]>(rows);
            var newOrder = new OrderRequestDto()
            {
                IdStatus = 1,
                IdUser = user.Data.Id
            };
            var createdOrder = await _orderService.Create(newOrder);
            var i = 0;
            
            while (orderFromJson != null && i < orderFromJson.Length)
            { 
                var shawarma = await _shawarmaService.GetByName(orderFromJson[i].Name);

                var orderShawa = new OrderShawarmaRequestDto
                {
                    OrderId = createdOrder.Data.Id,
                    ShawarmaId = shawarma.Data.Id,
                    Number = Convert.ToInt32(orderFromJson[i].Number)
                };
                
                await _orderShawarmaService.Create(orderShawa);
                i++;
            }
        }

        public async Task<IndexViewModel<ShawarmaResponseDto>> GetPage( bool needOnlyActual, int page = 1)
        {
            var count = await _shawarmaService.Count();
            var viewPage = 
                await _shawarmaService.GetListByPage(_appSettingsConfiguration, needOnlyActual, page);

            var pageViewModel = new PageViewModel(count, page, _appSettingsConfiguration);
            var viewModel = new IndexViewModel<ShawarmaResponseDto>
            {
                PageViewModel = pageViewModel,
                Things = viewPage.Data
            };

            return viewModel;
        }
    }
}