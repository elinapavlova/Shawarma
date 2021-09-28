using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure
{
    public class ReportOrderRepository : IReportOrderRepository
    {
        private readonly Context _context;

        public ReportOrderRepository
        (
            Context context
        )
        {
            _context = context;
        }
        
        public async Task<ReportOrder> Create(ReportOrder report)
        {
            await _context.ReportOrders.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }
        
        public async Task<ReportOrder> GetById(int id)
        {
            var report = await _context.ReportOrders.FindAsync(id);
            return report;
        }
        
        public async Task<ICollection<ReportOrder>> GetList()
        {
            var reports = await _context.ReportOrders
                .OrderBy(r => r.Id)
                .ToListAsync();

            return reports;
        }
        
        public async Task<ReportOrder> Edit(ReportOrder editedReport)
        {
            var report = await _context.ReportOrders.FindAsync(editedReport.Id);
            _context.ReportOrders.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }
        
        public async Task<ReportOrder> Delete(int id)
        {
            var report = await _context.ReportOrders.FindAsync(id);
            _context.ReportOrders.Remove(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}