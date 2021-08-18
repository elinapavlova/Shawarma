using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Status;

namespace Infrastructure.Contracts
{
    public interface IStatusRepository
    {
        Task<ICollection<Status>> GetStatusList();

        Task<Status> CreateStatus(Status status);
        
        Task<Status> UpdateStatus(Status status);
        
        Task<Status> DeleteStatus(int id);

        Task<Status> GetStatusById(int id);
    }
}