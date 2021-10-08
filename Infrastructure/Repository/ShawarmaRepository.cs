using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Models.Shawarma;

namespace Infrastructure.Repository
{
    public class ShawarmaRepository : BaseRepository<Shawarma>, IShawarmaRepository
    {
        private readonly ApiContext _db;
        private readonly int _pageSize;
        
        public ShawarmaRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
            _db = context;
            _pageSize = appSettings.DefaultPageSize;
        }

        public async Task<ICollection<Shawarma>> GetActualShawarmaList()
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Where(s => s.IsActual == true)
                .ToListAsync();
            return shawarmas; 
        }

        public async Task<Shawarma> GetShawarmaByName(string name)
        {
            var shawarma = await _db.Shawarmas.FirstOrDefaultAsync(s => s.Name != null && name == s.Name);
            return shawarma;
        }

        private async Task<ICollection<Shawarma>> ApplyPaging(int pageSize, int page = 1)
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return shawarmas;
        }

        private async Task<ICollection<Shawarma>> ApplyPagingForActualShawarma(int pageSize, int page = 1)
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Where(s => s.IsActual == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

        public new Task<ICollection<Shawarma>> GetPage(int pageSize, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}