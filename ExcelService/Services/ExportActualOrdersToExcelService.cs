using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Export.Services.Contracts;
using Models.ViewModels;
using Services.Contracts;

namespace Export.Services
{
    public class ExportActualOrdersToExcelService : IExportActualOrdersToExcelService
    {
        private readonly IOrderService _orderService;
        private readonly IStatusService _statusService;
        private readonly IUserService _userService;
        private readonly IShawarmaService _shawarmaService;

        public ExportActualOrdersToExcelService
        (
            IOrderService orderService,
            IStatusService statusService,
            IUserService userService,
            IShawarmaService shawarmaService
        )
        {
            _orderService = orderService;
            _statusService = statusService;
            _userService = userService;
            _shawarmaService = shawarmaService;
        }
        
        public async Task<ICollection<ExportActualOrdersViewModel>> PrepareOrderDataForExport()
        {
            //var shawarmas = await _orderService.GetActualList(DateTime.Now);
            var shawarmas = await _orderService.GetListByPage(3);

            ICollection<ExportActualOrdersViewModel> orders = new List<ExportActualOrdersViewModel>();

            try
            {
                foreach (var shawarma in shawarmas.Data)
                {
                    var orderShawa = new List<OrderShawarmaViewModel>();
                    var status = await _statusService.GetById(shawarma.IdStatus);
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
                        Status = status.Data.Name,
                        Comment = shawarma.Comment,
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

        public async Task<StringContent> CreateStringContentForExportOrders()
        {
            var orders = await PrepareOrderDataForExport();
            var content = new StringContent
            (
                JsonSerializer.Serialize(orders),
                Encoding.UTF8,
                "application/json"
            );

            return content;
        }
    }
}