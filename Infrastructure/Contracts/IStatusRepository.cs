using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;
using Models.Status;

namespace Infrastructure.Contracts
{
    public interface IStatusRepository
    {
        Task<ICollection<Status>> GetStatusList();

        void Create(Status status);
        
        void Update(Status status);
        
        void Delete(Status status);

        void GetStatusById(int id);
    }
}