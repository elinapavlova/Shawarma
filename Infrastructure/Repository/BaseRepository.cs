using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace Infrastructure.Repository
{
    public class BaseRepository<TModel> : IBaseRepository<TModel>
        where TModel : BaseModel
    {
        private readonly ApiContext _context;
        private readonly int _pageSize;
        protected BaseRepository
        (
            ApiContext context,
            IConfiguration configuration
        )
        {
            _context = context;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }
        
        protected virtual IQueryable<TModel> GetDataSet()
            => _context.Set<TModel>().AsNoTracking();

        public async Task<TModel> GetById(int id)
        {
            var data = await GetDataSet().FirstOrDefaultAsync(u => u.Id == id);
            return data;
        }

        public async Task<TModel> Create(TModel data)
        {
            await _context.Set<TModel>().AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TModel> Edit(TModel data)
        {
            _context.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TModel> Delete(int id)
        {
            var data = await GetDataSet().FirstOrDefaultAsync(u => u.Id == id);
            _context.Set<TModel>().Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ICollection<TModel>> GetList()
        {
            var data = await GetDataSet().ToListAsync();
            return data;
        }

        public async Task<int> Count()
        {
            var count = await GetList();
            return count.Count;
        }

        public async Task<ICollection<TModel>> GetPage(int pageSize, int page = 1)
        {
            var data = await _context.Set<TModel>()
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();
            return data;
        }
    }
}