using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Export.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ExcelController : BaseController
    {
        private readonly IExportActualOrdersToExcelService _exportService;
        private readonly IImportShawarmaFromExcelService _importService;
        private readonly Uri _baseAddress;
        public ExcelController
        (
            IExportActualOrdersToExcelService exportService,
            IImportShawarmaFromExcelService importService,
            IConfiguration configuration
        )
        {
            _exportService = exportService;
            _importService = importService;
            _baseAddress = new Uri(configuration["BaseAddress:ExcelUri"]);
        }
        
        [HttpPost("Export")]
        public async Task<FileContentResult> ExportToExcel()
        {
            using var client = new HttpClient();
            client.BaseAddress = _baseAddress;

            var response = await client.PostAsync("Excel/Export", 
                await _exportService.CreateStringContentForExportOrders());

            var result = await response.Content.ReadAsByteArrayAsync();

            return new FileContentResult(result, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"orders_{DateTime.Now}.xlsx"
            };
        }
        
        [HttpPost("Import")]
        public async Task<string> ImportFromExcel([FromForm]ICollection<IFormFile> files)
        {
            using var client = new HttpClient();
            client.BaseAddress = _baseAddress;
            
            var response = await client.PostAsync("Excel/Import", 
                await _importService.CreateMultipartContentForImportShawarma(files));

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}