using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Export.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Models.Shawarma;
using Models.ViewModels;
using OfficeOpenXml;
using Services.Contracts;

namespace Export.Services
{
    public class ImportShawarmaFromExcelService : IImportShawarmaFromExcelService
    {
        private readonly IMapper _mapper;
        private readonly IShawarmaService _shawarmaService;
        
        public ImportShawarmaFromExcelService
        (
            IMapper mapper,
            IShawarmaService shawarmaService
        )
        {
            _mapper = mapper;
            _shawarmaService = shawarmaService;
        }
        
        public async Task ImportFromExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Если файл пустой
                if (file?.Length <= 0 || file == null)
                    throw new Exception("Пустой файл!");
                
                var stream = file.OpenReadStream();

                using var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets.First();
                var totalRows = workSheet.Dimension.Rows;

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                for (var i = 2; i <= totalRows; i++)
                {
                    var name = workSheet.Cells[i, 1].Value.ToString();
                    var cost = workSheet.Cells[i, 2].Value.ToString();
                    var shawarma = ValidateExcel(name, cost);

                    if (shawarma == null)
                        throw new Exception($"Ошибка на строке {i}.");
                    
                    var newShawarma = _mapper.Map <ShawarmaRequestDto>(shawarma);
                    await _shawarmaService.Create(newShawarma);
                }
                scope.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static ShawarmaImportViewModel ValidateExcel(string _name, string _cost)
        {
            int outCost;
            
            var name = string.IsNullOrEmpty(_name);
            var cost = int.TryParse(_cost, out outCost);

            if (name || !cost)
                return null;
            
            var result = new ShawarmaImportViewModel
            {
                Name = _name,
                Cost = outCost
            };
            return result;
        }
    }
}