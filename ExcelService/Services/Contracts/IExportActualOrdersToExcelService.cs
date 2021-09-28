using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Models.ViewModels;

namespace Export.Services.Contracts
{
    public interface IExportActualOrdersToExcelService
    {
        Task<ICollection<ExportActualOrdersViewModel>> PrepareOrderDataForExport();
        Task<StringContent> CreateStringContentForExportOrders();
    }
}