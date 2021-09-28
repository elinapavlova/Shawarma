using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;

namespace ExcelService.Services.Contracts
{
    public interface IExportActualOrdersToExcelService
    {
        Task<byte[]> ExportToExcel(ICollection<ExportActualOrdersViewModel> json);
    }
}