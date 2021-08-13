using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Shawarma;

namespace Infrastructure.Contracts
{
    public interface IShawarmaRepository
    {
        Task<ICollection<Shawarma>> GetShawarmaList();

        void CreateShawarma(Shawarma shawarma);
        
        void UpdateShawarma(int id, Shawarma shawarma);
        
        void DeleteShawarma(int id);

        Task<Shawarma> GetShawarmaById(int id);
    }
}