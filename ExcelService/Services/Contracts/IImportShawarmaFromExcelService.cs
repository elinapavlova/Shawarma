using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Export.Services.Contracts
{
    public interface IImportShawarmaFromExcelService
    {
        Task<MultipartFormDataContent> CreateMultipartContentForImportShawarma(IEnumerable<IFormFile> files);
    }
}