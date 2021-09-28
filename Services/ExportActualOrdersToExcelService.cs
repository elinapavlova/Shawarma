using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ClosedXML.Excel;
using ExcelService.Services.Contracts;
using Models;
using Models.Dtos;
using Models.ViewModels;
using Services.Contracts;

namespace Services
{
    public class ExportActualOrdersToExcelService : IExportActualOrdersToExcelService
    {
        private readonly IReportService _reportService;
        private readonly IReportOrderService _reportOrderService;
        
        public ExportActualOrdersToExcelService
        (
            IReportService reportService,
            IReportOrderService reportOrderService
        )
        {
            _reportService = reportService;
            _reportOrderService = reportOrderService;
        }
        
        public async Task<byte[]> ExportToExcel(ICollection<ExportActualOrdersViewModel> json)
        {
            var i = 0; // счетчик строк
            var report = new ReportDto { WasCreated = DateTime.Now};

            using var workbook = new XLWorkbook(XLEventTracking.Disabled);
            var worksheet = CreateWorksheet(workbook);

            try
            {
                foreach (var order in json)
                {
                    await CreateWorksheetCellValue(i, order, worksheet);
                    i++;
                }
                
                await using var stream = new MemoryStream();
                
                workbook.SaveAs(stream);
                stream.Flush();
                
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                //var reportId = await CreateReport(report);
                //await CreateReportOrders(reportId, json);
                scope.Complete();
                
                return stream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static IXLWorksheet CreateWorksheet(IXLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Orders");

            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Дата";
            worksheet.Cell("C1").Value = "Статус";
            worksheet.Cell("D1").Value = "Клиент";
            worksheet.Cell("E1").Value = "Шаурма";
            worksheet.Cell("F1").Value = "Стоимость";
                
            worksheet.Row(1).Style.Font.Bold = true;
            
            return worksheet;
        }

        private static async Task CreateWorksheetCellValue(int i, Order order, IXLWorksheet worksheet)
        {
            if (order.Cost < 0) throw new Exception 
                ($"Ошибка в заказе {order.Id}. Стоимость заказа не может быть меньше 0!");
                    
            var shawarmasInOrder = order.OrderShawarmas
                .Aggregate("", (current, shawa) 
                    => current + "шаурма " + shawa.Shawarma + " " + shawa.Number + " шт.; ");

            worksheet.Cell(i + 2, 1).Value = order.Id;
            worksheet.Cell(i + 2, 2).Value = order.Date;
            worksheet.Cell(i + 2, 3).Value = order.Status;
            worksheet.Cell(i + 2, 4).Value = order.User;
            worksheet.Cell(i + 2, 5).Value = shawarmasInOrder;
            worksheet.Cell(i + 2, 6).Value = order.Cost;
        }

        private async Task<int> CreateReport(ReportDto report)
        {
            var result = await _reportService.Create(report);
            return result.Id;
        }

        private async Task CreateReportOrders(int reportId, IEnumerable<ExportActualOrdersViewModel> orders)
        {
            var reportOrder = new ReportOrderDto
            {
                ReportId = reportId
            };
            
            foreach (var order in orders)
            {
                reportOrder.OrderId = order.Id;
                await _reportOrderService.Create(reportOrder);
            }
        }
    }
}