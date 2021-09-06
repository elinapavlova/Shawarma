using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Shawarma;

namespace Infrastructure.Contracts
{
    public interface IShawarmaRepository
    {
        Task<ICollection<Shawarma>> GetShawarmaList();
        Task<ICollection<Shawarma>> GetActualShawarmaList();
        Task<Shawarma> CreateShawarma(Shawarma shawarma);
        Task<Shawarma> UpdateShawarma(Shawarma shawarma);
        Task<Shawarma> DeleteShawarma(int id);
        Task<Shawarma> GetShawarmaById(int id);
        Task<Shawarma> GetShawarmaByName(string name);
    }
}