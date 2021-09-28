using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.Dtos;

namespace Services.Contracts
{
    public interface IReportOrderService
    {
        Task<ReportOrderDto> Create(ReportOrderDto reportDto);
        Task<ReportOrderDto> GetById(int id);
        Task<ICollection<ReportOrderDto>> GetList();
        Task<ReportOrderDto> Edit(ReportOrderDto editedReport);
        Task<ReportOrderDto> Delete(int id);
    }
}