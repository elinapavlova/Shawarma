using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Models.OrderShawarma;

namespace Infrastructure.Repository
{
    public class OrderShawarmaRepository : BaseRepository<OrderShawarma>, IOrderShawarmaRepository
    {
        public OrderShawarmaRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
        }
    }
}