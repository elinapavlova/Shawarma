using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Models.Status;

namespace Infrastructure.Repository
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        public StatusRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
        }
    }
}