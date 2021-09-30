using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;

namespace Services.Contracts
{
    public interface IExportActualOrdersToExcelService
    {
        Task<byte[]> PostOrders();
        Task<ICollection<ExportActualOrdersViewModel>> PrepareOrderDataForExport();
    }
}