using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Status;

namespace Infrastructure.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApiContext _db;

        public StatusRepository(ApiContext context)
        {
            _db = context;
        }
        
        public async Task<ICollection<Status>> GetStatusList()
        {
            var statuses = await _db.Statuses.ToListAsync();
            return statuses;
        }

        public async Task<Status> CreateStatus(Status status)
        {
            await _db.Statuses.AddAsync(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> UpdateStatus(Status newStatus)
        {
            var status = await _db.Statuses.FindAsync(newStatus.Id);
            
            status.Name = newStatus.Name;

            _db.Statuses.Update(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> DeleteStatus(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            _db.Statuses.Remove(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> GetStatusById(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            return status;
        }
    }
}