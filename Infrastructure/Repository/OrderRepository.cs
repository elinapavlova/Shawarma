using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.OrderShawarma;

namespace Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApiContext _db;

        public OrderRepository(ApiContext context, AppSettingsOptions appSettings) : base (context, appSettings)
        {
            _db = context;
        }
        
        public async Task<ICollection<Order>> GetActualList(DateTime date)
        {
            var orders = await _db.Orders
                .Where(o => o.Date.Day == date.Day)
                .OrderBy(o => o.Id)
                .ToListAsync();

            foreach (var order in orders)
                order.OrderShawarmas = await GetOrderShawarmas(order.Id);
            
            return orders;
        }

        public async Task<ICollection<Order>> GetActualListByPage(DateTime date, int pageSize, int page = 1)
        {
            var orders = await _db.Orders
                .Where(o => o.Date.Date == date.Date)
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            foreach (var order in orders)
                order.OrderShawarmas = await GetOrderShawarmas(order.Id);
            
            return orders;
        }

        public async Task<Order> GetByIdWithShawarmas(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null) 
                return null;

            order.OrderShawarmas = await GetOrderShawarmas(order.Id);
            
            return order;
        }
        
        private async Task<List<OrderShawarma>> GetOrderShawarmas(int idOrder)
        {
            var orderShawasOfOrder = _db.OrderShawarmas
                .Where(o => o.OrderId == idOrder)
                .OrderBy(o => o.Id)
                .ToList();

            return orderShawasOfOrder;
        }
    }
}