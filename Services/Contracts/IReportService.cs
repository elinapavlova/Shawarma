using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Dtos;

namespace Services.Contracts
{
    public interface IReportService
    {
        Task<ReportDto> Create(ReportDto reportDto);
        Task<ReportDto> GetById(int id);
        Task<ReportDto> GetByDate(DateTime date);
        Task<ICollection<ReportDto>> GetList();
        Task<ReportDto> Edit(ReportDto editedReport);
        Task<ReportDto> Delete(int id);
    }
}