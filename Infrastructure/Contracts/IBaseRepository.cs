using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Infrastructure.Contracts
{
    public interface IBaseRepository<TModel>
        where TModel : BaseModel
    {
        Task<TModel> GetById(int id);
        Task<TModel> Create(TModel data);
        Task<TModel> Edit(TModel data);
        Task<TModel> Delete(int id);
        Task<ICollection<TModel>> ApplyPaging(ICollection<TModel> source, int pageSize, int page = 1);
        Task<ICollection<TModel>> GetList();
        Task<int> Count();
        Task<ICollection<TModel>> GetPage(int pageSize, int page = 1);
    }
}