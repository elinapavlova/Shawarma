using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Export.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ExportController : BaseController
    {
        private readonly IExportActualOrdersToExcelService _exportService;
        private readonly IImportShawarmaFromExcelService _importService;
        
        public ExportController
        (
            IExportActualOrdersToExcelService exportService,
            IImportShawarmaFromExcelService importService
        )
        {
            _exportService = exportService;
            _importService = importService;
        }
        
        [HttpGet]
        public async Task<byte[]> ExportToExcel()
        {
            var result = await _exportService.ExportToExcel();
            return result;
        }
        
        [HttpPost]
        public async Task ImportFromExcel(IFormFile file)
        {
            await _importService.ImportFromExcel(file);
        }
    }
}