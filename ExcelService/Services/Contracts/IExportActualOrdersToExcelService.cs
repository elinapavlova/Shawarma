using System.Threading.Tasks;

namespace Export.Services.Contracts
{
    public interface IExportActualOrdersToExcelService
    {
        Task<byte[]> ExportToExcel();
    }
}