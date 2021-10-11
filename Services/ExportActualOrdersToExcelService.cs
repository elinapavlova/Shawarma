using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Models.Enums;
using Models.ViewModels;
using Services.Contracts;

namespace Services
{
    public class ExportActualOrdersToExcelService : IExportActualOrdersToExcelService
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IShawarmaService _shawarmaService;
        private readonly IHttpClientFactory _clientFactory;

        public ExportActualOrdersToExcelService
        (
            IOrderService orderService,
            IUserService userService,
            IShawarmaService shawarmaService,
            IHttpClientFactory clientFactory 
        )
        {
            _orderService = orderService;
            _userService = userService;
            _shawarmaService = shawarmaService;
            _clientFactory = clientFactory;
        }
        
        public async Task<ICollection<ExportActualOrdersViewModel>> PrepareOrderDataForExport()
        {
            var shawarmas = await _orderService.GetActualList(DateTime.Now);
            ICollection<ExportActualOrdersViewModel> orders = new List<ExportActualOrdersViewModel>();

            try
            {
                foreach (var shawarma in shawarmas.Data)
                {
                    var orderShawa = new List<OrderShawarmaViewModel>();
                    var status = Enum.GetName(typeof(StatusesEnum), shawarma.IdStatus);
                    var user = await _userService.GetById(shawarma.IdUser);

                    foreach (var shawa in shawarma.OrderShawarmas)
                    {
                        var orderShawarma = await _shawarmaService.GetById(shawa.ShawarmaId);
                        orderShawa.Add(new OrderShawarmaViewModel
                        {
                            Shawarma = orderShawarma.Data.Name,
                            Number = shawa.Number
                        });
                    }

                    orders.Add(new ExportActualOrdersViewModel
                    {
                        Id = shawarma.Id,
                        Date = shawarma.Date,
                        User = user.Data.UserName,
                        Status = status,
                        Cost = shawarma.Cost,
                        OrderShawarmas = orderShawa
                    });
                }

                await using var stream = new MemoryStream();
                return orders;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<byte[]> PostOrders()
        {
            var orders = await PrepareOrderDataForExport();
            var content = new StringContent
            (
                JsonSerializer.Serialize(orders),
                Encoding.UTF8,
                "application/json"
            );

            using var client = _clientFactory.CreateClient("Excel");

            var response = await client.PostAsync("Export", content);
            var result = await response.Content.ReadAsByteArrayAsync();

            return result;
        }
    }
}