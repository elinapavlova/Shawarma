using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Database;
using Microsoft.AspNetCore.Http;
using Models;
using Models.Dtos;
using Models.ViewModels;
using OfficeOpenXml;
using Services.Contracts;

namespace Services
{
    public class ImportShawarmaFromExcelService : IImportShawarmaFromExcelService
    {
        private readonly IMapper _mapper;
        private readonly Context _db;
        
        public ImportShawarmaFromExcelService
        (
            Context context,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _db = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        
        public async Task<ImportResult> ImportFromExcel(IFormFile file)
        {
            var result = new ImportResult();
            try
            {
                // Проверка файла
                var validatedFile = ValidateFile(file);
                if (validatedFile.ErrorType.HasValue)
                    return validatedFile;

                var stream = file.OpenReadStream();

                using var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets.First();
                var totalRows = workSheet.Dimension.Rows;

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                for (var i = 2; i <= totalRows; i++)
                {
                    var name = workSheet.Cells[i, 1].Value;
                    var cost = workSheet.Cells[i, 2].Value;

                    var shawarma = ValidateRow(name, cost);
                    
                    // Если данные невалидны
                    if (shawarma == null)
                    {
                        result.ErrorType = ImportErrorType.BadData;
                        result.Message = $"Ошибка на строке {i}";
                        return result;
                    }
                    
                    var newShawarma = _mapper.Map<ShawarmaDto>(shawarma);
                    var shawa = _db.Shawarmas
                        .FirstOrDefault(s => s.Name != null && newShawarma.Name == s.Name);
                    
                    if (shawa != null)
                        continue;
                    
                    await _db.Shawarmas.AddAsync(newShawarma);
                    await _db.SaveChangesAsync();
                }
                scope.Complete();
                result.Message = "Successful";
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static ImportResult ValidateFile(IFormFile file)
        {
            var result = new ImportResult();
            
            // Если файл пустой
            if (file?.Length <= 0 || file == null)
            {
                result.ErrorType = ImportErrorType.EmptyFile;
                result.Message = "Пустой файл";
                return result;
            }

            var extension = Path.GetExtension(file.FileName);

            // Если не excel-страница
            if (extension.ToLower().Equals(".xlsx")) 
                return result;
            
            result.ErrorType = ImportErrorType.InvalidFileExtension;
            result.Message = "Недопустимое расширение файла";
            return result;

        }
        
        private static ShawarmaImportViewModel ValidateRow(object _name, object _cost)
        {
            int outCost;

            if (_name == null || _cost == null)
                return null;
            
            var name = string.IsNullOrEmpty(_name.ToString());
            var cost = int.TryParse(_cost.ToString(), out outCost);

            if (name || !cost)
                return null;
            
            var result = new ShawarmaImportViewModel
            {
                Name = _name.ToString(),
                Cost = outCost
            };
            return result;
        }
        
    }
}