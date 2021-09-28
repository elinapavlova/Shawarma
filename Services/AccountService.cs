using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
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
        private readonly IUserService _userService;
        private readonly int _pageSize;

        public AccountService
        (
            IOrderService orderService,
            IShawarmaService shawarmaService,
            IOrderShawarmaService orderShawarmaService,
            IUserService userService,
            IConfiguration configuration
        )
        {
            _orderService = orderService;
            _shawarmaService = shawarmaService;
            _orderShawarmaService = orderShawarmaService;
            _userService = userService;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async void CreateOrder(ResultContainer<UserResponseDto> user, string rows)
        {
            var orderFromJson = JsonSerializer.Deserialize<CreateOrderViewModel[]>(rows);
            var newOrder = new OrderRequestDto
            {
                IdStatus = 1,
                IdUser = user.Data.Id
            };
            
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
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
            scope.Complete();
        }

        public async Task<IndexViewModel<ShawarmaResponseDto>> GetShawarmaPage(bool needOnlyActual, int page = 1)
        {
            var count = await _shawarmaService.Count();
            var viewPage = await _shawarmaService.GetListByPage(_pageSize, needOnlyActual, page);

            var pageViewModel = new PageViewModel(count, page, _pageSize);
            var viewModel = new IndexViewModel<ShawarmaResponseDto>
            {
                PageViewModel = pageViewModel,
                Things = viewPage.Data
            };

            return viewModel;
        }
        
        public async Task<IndexViewModel<OrderResponseDto>> GetOrdersPage( bool needOnlyActual, int page = 1)
        {
            var count = await _orderService.Count();
            ResultContainer<ICollection<OrderResponseDto>> viewPage;
            
            if (needOnlyActual)
                viewPage = await _orderService.GetActualListByPage(DateTime.Today, _pageSize, page);
            else
                viewPage = await _orderService.GetListByPage(_pageSize, page);
            
            var pageViewModel = new PageViewModel(count, page, _pageSize);
            var viewModel = new IndexViewModel<OrderResponseDto>
            {
                PageViewModel = pageViewModel,
                Things = viewPage.Data
            };
            return viewModel;
        }
        
        public async Task<IndexViewModel<OrderResponseDto>> GetOrdersByUserPage(int id, int page = 1)
        {
            var count = await _orderService.Count();
            var user = await _userService.GetById(id);
            var pageViewModel = new PageViewModel(count, page, _pageSize);
            
            var viewModel = new IndexViewModel<OrderResponseDto>
            {
                PageViewModel = pageViewModel,
                Things = user.Data.Orders
            };
            
            return viewModel;
        }
    }
}