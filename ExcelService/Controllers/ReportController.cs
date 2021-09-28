using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ExcelService.Controllers
{
    [Route("Report")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportService _service;

        public ReportController
        (
            IReportService service
        )
        {
            _service = service;
        }
    }
}