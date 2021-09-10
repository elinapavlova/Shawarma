using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Shawarma;

namespace Infrastructure.Contracts
{
    public interface IShawarmaRepository : IBaseRepository<Shawarma>
    {
        Task<Shawarma> GetShawarmaByName(string name);
        Task<ICollection<Shawarma>> GetPage(int pageSize,  bool needOnlyActual, int page = 1);
    }
}