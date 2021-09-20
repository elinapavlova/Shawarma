using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Export.Services.Contracts
{
    public interface IImportShawarmaFromExcelService
    {
        Task ImportFromExcel(IFormFile file);
    }
}