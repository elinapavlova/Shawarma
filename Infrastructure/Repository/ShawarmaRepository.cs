using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Shawarma;

namespace Infrastructure.Repository
{
    public class ShawarmaRepository : IShawarmaRepository
    {
        private readonly ApiContext _db;
        private readonly int _pageSize;
        
        public ShawarmaRepository
        (
            ApiContext context, 
            IConfiguration configuration
        )
        {
            _db = context;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<ICollection<Shawarma>> GetActualShawarmaList()
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Where(s => s.IsActual == true)
                .ToListAsync();
            return shawarmas; 
        }

        public async Task<int> Count()
        {
            var count = await _db.Shawarmas.CountAsync();
            return count;
        }

        public async Task<Shawarma> GetShawarmaByName(string name)
        {
            var shawarma = await _db.Shawarmas.FirstOrDefaultAsync(s => s.Name != null && name == s.Name);
            return shawarma;
        }

        public async Task<Shawarma> GetById(int id)
        {
            var shawarma = await _db.Shawarmas.FindAsync(id);
            return shawarma;
        }

        public async Task<Shawarma> Create(Shawarma shawarma)
        {
            await _db.Shawarmas.AddAsync(shawarma);
            await _db.SaveChangesAsync();
            return shawarma;
        }

        public async Task<Shawarma> Edit(Shawarma newShawarma)
        {
            var shawarma = await _db.Shawarmas
                .FirstOrDefaultAsync(s => s.Name != null && newShawarma.Name == s.Name);

            if (shawarma == null)
                return null;

            shawarma.Name = newShawarma.Name;
            shawarma.Cost = newShawarma.Cost;
            shawarma.IsActual = newShawarma.IsActual;

            _db.Shawarmas.Update(shawarma);
            await _db.SaveChangesAsync();

            return shawarma;
        }

        public async Task<Shawarma> Delete(int id)
        {
            var shawarma = await _db.Shawarmas.FindAsync(id);
            _db.Shawarmas.Remove(shawarma);
            await _db.SaveChangesAsync();
            return shawarma;
        }

        public async Task<ICollection<Shawarma>> ApplyPaging(int pageSize, int page = 1)
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return shawarmas;
        }

        public async Task<ICollection<Shawarma>> ApplyPagingForActualShawarma(int pageSize, int page = 1)
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Where(s => s.IsActual == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return shawarmas;
        }
        
        public async Task<ICollection<Shawarma>> GetList()
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(u => u.Id)
                .ToListAsync();
            return shawarmas;
        }

        public async Task<ICollection<Shawarma>> GetPage(int pageSize, bool needOnlyActual, int page = 1)
        {
            ICollection<Shawarma> result;
            
            if (needOnlyActual)
                result = await ApplyPagingForActualShawarma(_pageSize, page);
            else
                result = await ApplyPaging(_pageSize, page);
            
            return result;
        }

        public Task<ICollection<Shawarma>> GetPage(int pageSize, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}