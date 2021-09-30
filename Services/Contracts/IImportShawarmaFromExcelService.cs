using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IImportShawarmaFromExcelService
    {
        Task<string> ImportShawarmas(IEnumerable<IFormFile> files);
    }
}