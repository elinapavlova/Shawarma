using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Shawarma;

namespace Infrastructure.Repository
{
    public class ShawarmaRepository : IShawarmaRepository
    {
        private readonly ApiContext _db;
        
        public ShawarmaRepository(ApiContext context)
        {
            _db = context;
        }
        public async Task<ICollection<Shawarma>> GetShawarmaList()
        {
            var shawarmas = await _db.Shawarmas.ToListAsync();
            return shawarmas; 
        }

        public async void CreateShawarma(Shawarma shawarma)
        {
            await _db.Shawarmas.AddAsync(shawarma);
            await _db.SaveChangesAsync();
        }

        public void UpdateShawarma(Shawarma newShawarma)
        {
            var shawarma = _db.Shawarmas.FindAsync(newShawarma.Id).Result;
            
            shawarma.Name = newShawarma.Name;
            shawarma.Cost = newShawarma.Cost;
            shawarma.IsActual = newShawarma.IsActual;

            _db.Shawarmas.Update(shawarma);
            _db.SaveChangesAsync();
        }

        public async void DeleteShawarma(int id)
        {
            var shawarma = _db.Shawarmas.FindAsync(id).Result;
            _db.Shawarmas.Remove(shawarma);
            await _db.SaveChangesAsync();
        }

        public async Task<Shawarma> GetShawarmaById(int id)
        {
            var shawarma = await _db.Shawarmas.FindAsync(id);
            return shawarma;
        }
    }
}