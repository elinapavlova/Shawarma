using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Infrastructure.Contracts
{
    public interface IReportRepository
    {
        Task<Report> Create(Report report);
        Task<Report> GetById(int id);
        Task<Report> GetByDate(DateTime date);
        Task<ICollection<Report>> GetList();
        Task<Report> Edit(Report editedReport);
        Task<Report> Delete(int id);
    }
}