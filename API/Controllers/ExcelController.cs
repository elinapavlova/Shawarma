﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ExcelController : BaseController
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
        
        /// <summary>
        /// Export today orders to excel report
        /// </summary>
        /// <returns></returns>
        [HttpPost("Export")]
        public async Task<FileContentResult> ExportToExcel()
        {
            return new FileContentResult(await _exportService.PostOrders(), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"orders_{DateTime.Now}.xlsx"
            };
        }
        
        /// <summary>
        /// Import all kinds of shawarma from excel to database
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("Import")]
        public async Task<string> ImportFromExcel([FromForm]ICollection<IFormFile> files)
        {
            return await _importService.ImportShawarmas(files);
        }
    }
}