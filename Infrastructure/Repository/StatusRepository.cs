using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Status;

namespace Infrastructure.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApiContext _db;
        private readonly int _appSettingsConfiguration;
        
        public StatusRepository
        (
            ApiContext context,
            IConfiguration configuration
        )
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<ICollection<Status>> ApplyPaging(ICollection<Status> source, int pageSize, int page = 1)
        {
            var statuses = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return statuses;
        }

        public async Task<ICollection<Status>> GetList()
        {
            var statuses = await _db.Statuses
                .OrderBy(o => o.Id)
                .ToListAsync();
            return statuses;
        }

        public async Task<int> Count()
        {
            var count = await _db.Statuses.CountAsync();
            return count;
        }

        public async Task<ICollection<Status>> GetPage(int pageSize, int page = 1)
        {
            var source = await GetList();
            var result = await ApplyPaging(source, _appSettingsConfiguration, page);
            return result;
        }

        public async Task<Status> Create(Status status)
        {
            await _db.Statuses.AddAsync(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> Edit(Status newStatus)
        {
            var status = await _db.Statuses.FindAsync(newStatus.Id);
            
            status.Name = newStatus.Name;

            _db.Statuses.Update(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> Delete(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            _db.Statuses.Remove(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<Status> GetById(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            return status;
        }
    }
}