using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExcelService.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Services.Contracts;

namespace ExcelService.Controllers
{
    [Route("Excel")]
    [ApiController]
    public class ExcelController : Controller
    {
        private readonly IExportActualOrdersToExcelService _exportService;
        private readonly IImportShawarmaFromExcelService _importService;

        public ExcelController
        (
            IExportActualOrdersToExcelService exportService,
            IImportShawarmaFromExcelService importService
        )
        {
            _exportService = exportService;
            _importService = importService;
        }
        
        [HttpPost("Export")]
        public async Task<FileContentResult> ExportToExcel(ICollection<ExportActualOrdersViewModel> shawarma)
        {
            var result = await _exportService.ExportToExcel(shawarma);

            return new FileContentResult(result, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"orders_{DateTime.Now}.xlsx"
            };
        }
        
        [HttpPost("Import")]
        public async Task<ImportResult> ImportFromExcel(IFormFile file)
        {
            var result = await _importService.ImportFromExcel(file);
            return result;
        }
    }
}