using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Models.Role;

namespace Infrastructure.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
        }
    }
}