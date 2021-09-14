using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Order;

namespace Infrastructure.Contracts
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<ICollection<Order>> GetActualList(DateTime date);
        Task<ICollection<Order>> GetActualListByPage(DateTime date, int pageSize, int page = 1);
    }
}