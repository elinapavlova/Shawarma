using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Status;

namespace Infrastructure.Contracts
{
    public interface IStatusRepository : IBaseRepository<Status>
    {
    }
}