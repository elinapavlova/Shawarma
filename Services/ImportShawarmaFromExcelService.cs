using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Services.Contracts;

namespace Services
{
    public class ImportShawarmaFromExcelService : IImportShawarmaFromExcelService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public ImportShawarmaFromExcelService
        (
            IHttpClientFactory clientFactory
        )
        {
            _clientFactory = clientFactory;
        }

        private static async Task<MultipartFormDataContent> CreateMultipartContentForImportShawarma(IEnumerable<IFormFile> files)
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

        public async Task<string> ImportShawarmas(IEnumerable<IFormFile> files)
        {
            using var client = _clientFactory.CreateClient("Excel");
            
            var response = await client.PostAsync("Import", await CreateMultipartContentForImportShawarma(files));

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}