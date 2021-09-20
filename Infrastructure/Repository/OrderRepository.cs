using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Order;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiContext _db;
        private readonly int _pageSize;
        public OrderRepository
        (
            ApiContext context,
            IConfiguration configuration
        )
        {
            _db = context;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }
        
        public async Task<ICollection<Order>> GetActualList(DateTime date)
        {
            var orders = await _db.Orders
                .Where(o => o.Date.Day == DateTime.Today.Day)
                .OrderBy(o => o.Id)
                .ToListAsync();
            
            var orderShawas = await _db.OrderShawarmas.ToListAsync();
            
            if (orderShawas.Count == 0 || orders.Count == 0)
                return orders;

            foreach (var order in orders)
            {
                var orderShawasOfOrder = orderShawas
                    .Where(o => o.OrderId == order.Id)
                    .ToList();
                
                order.OrderShawarmas = orderShawasOfOrder;
            }
            
            return orders;
        }

        public async Task<ICollection<Order>> GetActualListByPage(DateTime date, int pageSize, int page = 1)
        {
            var source = await GetActualList(date);
            var result = await ApplyPaging(source, _pageSize, page);
            return result;
        }

        public async Task<ICollection<Order>> GetPage(int pageSize, int page = 1)
        {
            var source = await GetList();
            var result = await ApplyPaging(source, _pageSize, page);
            return result;
        }

        public async Task<ICollection<Order>> ApplyPaging(ICollection<Order> source, int pageSize, int page = 1)
        {
            var shawarmas = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return shawarmas;
        }

        public async Task<ICollection<Order>> GetList()
        {
            var orders = await _db.Orders
                .OrderBy(o => o.Id)
                .ToListAsync();
            
            var orderShawas = await _db.OrderShawarmas.ToListAsync();
            
            if (orderShawas.Count == 0)
                return orders;

            foreach (var order in orders)
            {
                var orderShawasOfOrder = orderShawas
                    .Where(o => o.OrderId == order.Id)
                    .ToList();
                
                order.OrderShawarmas = orderShawasOfOrder;
            }
            
            return orders;
        }

        public async Task<int> Count()
        {
            var count = await _db.Orders.CountAsync();
            return count;
        }

        public async Task<Order> Create(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return order;
        }
        
        public async Task<Order> Edit(Order newOrder)
        {
            var order = await _db.Orders.FindAsync(newOrder.Id);
            
            order.Comment = newOrder.Comment;
            order.Date = newOrder.Date;
            order.IdStatus = newOrder.IdStatus;
            order.Cost = newOrder.Cost;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return order;
        }
        
        public async Task<Order> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            _db.Remove(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetById(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null) 
                return null;

            var orderShawas = await _db.OrderShawarmas.ToListAsync();
            
            if (orderShawas.Count == 0)
                return order;
            
            var orderShawasOfOrder = orderShawas.Where(o => o.OrderId == order.Id).ToList();
            
            if (orderShawasOfOrder.Count == 0)
                return order;

            order.OrderShawarmas = orderShawasOfOrder;
            
            return order;
        }
    }
}