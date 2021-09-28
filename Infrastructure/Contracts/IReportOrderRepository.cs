using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Infrastructure.Contracts
{
    public interface IReportOrderRepository
    {
        Task<ReportOrder> Create(ReportOrder report);
        Task<ReportOrder> GetById(int id);
        Task<ICollection<ReportOrder>> GetList();
        Task<ReportOrder> Edit(ReportOrder editedReport);
        Task<ReportOrder> Delete(int id);
    }
}