using System;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Export.Services.Contracts;
using Infrastructure.Result;
using Models.Shawarma;
using Models.Status;
using Models.User;
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
        
        public async Task<byte[]> ExportToExcel()
        {
            var shawarmas = await _orderService.GetActualList(DateTime.Now);
            var i = 0; // счетчик строк
            ResultContainer<StatusResponseDto> status;
            ResultContainer<UserResponseDto> user;
            ResultContainer<ShawarmaResponseDto> orderShawarma;

            using var workbook = new XLWorkbook(XLEventTracking.Disabled);
            
            var worksheet = workbook.Worksheets.Add("Orders");

            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Дата";
            worksheet.Cell("C1").Value = "Статус";
            worksheet.Cell("D1").Value = "Клиент";
            worksheet.Cell("E1").Value = "Шаурма";
            worksheet.Cell("F1").Value = "Стоимость";
                
            worksheet.Row(1).Style.Font.Bold = true;

            try
            {
                foreach (var shawarma in shawarmas.Data)
                {
                    if (shawarma.Cost < 0)
                        throw new Exception
                            ($"Ошибка в заказе {shawarma.Id}. Стоимость заказа не может быть меньше 0!");
                    
                    status = await _statusService.GetById(shawarma.IdStatus);
                    user = await _userService.GetById(shawarma.IdUser);
                    var shawarmasInOrder = "";

                    foreach (var shawa in shawarma.OrderShawarmas)
                    {
                        orderShawarma = await _shawarmaService.GetById(shawa.ShawarmaId);
                        shawarmasInOrder += "шаурма " + orderShawarma.Data.Name + " " + shawa.Number + " шт.; ";
                    }

                    worksheet.Cell(i + 2, 1).Value = shawarma.Id;
                    worksheet.Cell(i + 2, 2).Value = shawarma.Date;
                    worksheet.Cell(i + 2, 3).Value = status.Data.Name;
                    worksheet.Cell(i + 2, 4).Value = user.Data.Email;
                    worksheet.Cell(i + 2, 5).Value = shawarmasInOrder;
                    worksheet.Cell(i + 2, 6).Value = shawarma.Cost;

                    i++;
                }

                await using var stream = new MemoryStream();
                
                workbook.SaveAs(stream);
                stream.Flush();
                return stream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}