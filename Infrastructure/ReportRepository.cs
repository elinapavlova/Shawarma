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
            report.ReportOrders = await GetReportOrders(id);
            return report;
        }
        
        public async Task<Report> GetByDate(DateTime date)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => date == r.WasCreated);

            report.ReportOrders = await GetReportOrders(report.Id);
            
            return report;
        }
        
        public async Task<ICollection<Report>> GetList()
        {
            var reports = await _context.Reports
                .OrderBy(r => r.Id)
                .ToListAsync();

            if (reports.Count == 0)
                return reports;

            foreach (var report in reports)
                report.ReportOrders = await GetReportOrders(report.Id);

            return reports;
        }
        
        public async Task<Report> Edit(Report editedReport)
        {
            var report = await _context.Reports.FindAsync(editedReport.Id);
            
            report.WasCreated = editedReport.WasCreated;

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

        private async Task<List<ReportOrder>> GetReportOrders(int id)
        {
            var reportOrders =  _context.ReportOrders
                .Select(s => s)
                .ToList()
                .Where(r => r.ReportId == id)
                .ToList();

            return reportOrders;
        }
    }
}