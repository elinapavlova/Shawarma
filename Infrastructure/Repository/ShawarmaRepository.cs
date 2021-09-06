using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Shawarma;

namespace Infrastructure.Repository
{
    public class ShawarmaRepository : IShawarmaRepository
    {
        private readonly ApiContext _db;
        
        public ShawarmaRepository()
        {
            _db = new ApiContext();
        }
        public async Task<ICollection<Shawarma>> GetShawarmaList()
        {
            var shawarmas = await _db.Shawarmas.OrderBy(u => u.Id).ToListAsync();
            return shawarmas; 
        }
        
        public async Task<ICollection<Shawarma>> GetActualShawarmaList()
        {
            var shawarmas = await _db.Shawarmas
                .OrderBy(s => s.Id)
                .Where(s => s.IsActual == true)
                .ToListAsync();
            return shawarmas; 
        }

        public async Task<Shawarma> CreateShawarma(Shawarma shawarma)
        {
            await _db.Shawarmas.AddAsync(shawarma);
            await _db.SaveChangesAsync();

            return shawarma;
        }

        public async Task<Shawarma> UpdateShawarma(Shawarma newShawarma)
        {
            
            var shawarma = _db.Shawarmas.FirstOrDefault(s => s.Name != null && newShawarma.Name == s.Name);

            shawarma.Name = newShawarma.Name;
            shawarma.Cost = newShawarma.Cost;
            shawarma.IsActual = newShawarma.IsActual;

            _db.Shawarmas.Update(shawarma);
            await _db.SaveChangesAsync();

            return shawarma;
        }

        public async Task<Shawarma> DeleteShawarma(int id)
        {
            var shawarma = await _db.Shawarmas.FindAsync(id);
            _db.Shawarmas.Remove(shawarma);
            await _db.SaveChangesAsync();

            return shawarma;
        }

        public async Task<Shawarma> GetShawarmaById(int id)
        {
            var shawarma = await _db.Shawarmas.FindAsync(id);
            return shawarma;
        }
        
        public async Task<Shawarma> GetShawarmaByName(string name)
        {
            var shawarma = _db.Shawarmas.FirstOrDefault(s => s.Name != null && name == s.Name);

            return shawarma;
        }
    }
}