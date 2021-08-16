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

        public async void CreateStatus(Status status)
        {
            await _db.Statuses.AddAsync(status);
            await _db.SaveChangesAsync();
        }

        public void UpdateStatus(Status newStatus)
        {
            var status = _db.Statuses.FindAsync(newStatus.Id).Result;
            
            status.Name = newStatus.Name;

            _db.Statuses.Update(status);
            _db.SaveChangesAsync();
        }

        public async void DeleteStatus(int id)
        {
            var status = _db.Statuses.FindAsync(id).Result;
            _db.Statuses.Remove(status);
            await _db.SaveChangesAsync();
        }

        public async Task<Status> GetStatusById(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            return status ?? null;
        }
    }
}