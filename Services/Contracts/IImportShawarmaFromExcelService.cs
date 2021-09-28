using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models;

namespace Services.Contracts
{
    public interface IImportShawarmaFromExcelService
    {
        Task<ImportResult> ImportFromExcel(IFormFile file);
    }
}