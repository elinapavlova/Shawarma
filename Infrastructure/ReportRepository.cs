using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure
{
    public class ReportRepository : IReportRepository
    {
        private readonly Context _context;

        public ReportRepository
        (
            Context context
        )
        {
            _context = context;
        }
        
        public async Task<Report> Create(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }
        
        public async Task<Report> GetById(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            return report;
        }
        
        public async Task<Report> GetByDate(DateTime date)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => date == r.WasCreated);

            return report;
        }
        
        public async Task<ICollection<Report>> GetList()
        {
            var reports = await _context.Reports
                .OrderBy(r => r.Id)
                .ToListAsync();

            return reports;
        }

        public async Task<ICollection<Report>> GetListByPage(int pageSize, int page = 1)
        {
            var result = await ApplyPaging(pageSize, page);
            return result;
        }

        public async Task<Report> Edit(Report editedReport)
        {
            var report = await _context.Reports.FindAsync(editedReport.Id);
            
            report.WasCreated = editedReport.WasCreated;
            report.FileName = editedReport.FileName;
            report.Document = report.Document;

            _context.Reports.Update(report);
            await _context.SaveChangesAsync();

            return report;
        }
        
        public async Task<Report> Delete(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return report;
        }
        
        public async Task<ICollection<Report>> ApplyPaging(int pageSize, int page = 1)
        {
            var reports = _context.Reports
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return reports;
        }
    }
}