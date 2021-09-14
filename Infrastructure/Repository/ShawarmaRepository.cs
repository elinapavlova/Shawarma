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
        private readonly int _appSettingsConfiguration;
        
        public ShawarmaRepository
        (
            ApiContext context, 
            IConfiguration configuration
        )
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
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
            var shawarma = _db.Shawarmas.FirstOrDefault(s => s.Name != null && name == s.Name);
            return shawarma;
        }

        public async Task<Shawarma> GetById(int id)
        {
            var user = await _db.Shawarmas.FindAsync(id);
            return user;
        }

        public async Task<Shawarma> Create(Shawarma shawarma)
        {
            await _db.Shawarmas.AddAsync(shawarma);
            await _db.SaveChangesAsync();
            return shawarma;
        }

        public async Task<Shawarma> Edit(Shawarma newShawarma)
        {
            var shawarma = _db.Shawarmas.FirstOrDefault(s => s.Name != null && newShawarma.Name == s.Name);

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

        public async Task<ICollection<Shawarma>> ApplyPaging(ICollection<Shawarma> source, int pageSize, int page = 1)
        {
            var shawarmas = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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
            ICollection<Shawarma> source;
            if (needOnlyActual)
                source = await GetActualShawarmaList();
            else
                source = await GetList();
            var result = await ApplyPaging(source, _appSettingsConfiguration, page);
            return result;
        }
        
        public Task<ICollection<Shawarma>> GetPage(int pageSize, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}