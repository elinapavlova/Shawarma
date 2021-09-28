using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Export.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace Export.Services
{
    public class ImportShawarmaFromExcelService : IImportShawarmaFromExcelService
    {

        public async Task<MultipartFormDataContent> CreateMultipartContentForImportShawarma(IEnumerable<IFormFile> files)
        {
            var file = files.FirstOrDefault();

            if (file == null) 
                return null;
            
            var stream = new MemoryStream((int) file.Length);
            await file.CopyToAsync(stream);

            var byteArrayContent = new ByteArrayContent(stream.ToArray());
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(byteArrayContent, "file", file.FileName);

            return multipartContent;
        }
    }
}