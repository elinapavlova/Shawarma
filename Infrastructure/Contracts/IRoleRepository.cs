using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Role;

namespace Infrastructure.Contracts
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
    }
}