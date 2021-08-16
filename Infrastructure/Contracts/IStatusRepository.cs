using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;
using Models.Status;

namespace Infrastructure.Contracts
{
    public interface IStatusRepository
    {
        Task<ICollection<Status>> GetStatusList();

        void CreateStatus(Status status);
        
        void UpdateStatus(Status status);
        
        void DeleteStatus(int id);

        Task<Status> GetStatusById(int id);
    }
}