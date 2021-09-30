using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Order;
using Models.OrderShawarma;

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
            _pageSize = configuration.GetSection(AppSettingsOptions.AppSettings).Get<AppSettingsOptions>().DefaultPageSize;
        }
        
        public async Task<ICollection<Order>> GetActualList(DateTime date)
        {
            var orders = await _db.Orders
                .Where(o => o.Date.Day == DateTime.Today.Day)
                .OrderBy(o => o.Id)
                .ToListAsync();

            foreach (var order in orders)
                order.OrderShawarmas = await GetOrderShawarmas(order.Id);
            
            return orders;
        }

        public async Task<ICollection<Order>> GetActualListByPage(DateTime date, int pageSize, int page = 1)
        {
            var orders = await _db.Orders
                .Where(o => o.Date.Day == DateTime.Today.Day)
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return orders;
        }

        public async Task<ICollection<Order>> GetPage(int pageSize, int page = 1)
        {
            var result = await ApplyPaging(_pageSize, page);
            return result;
        }

        private async Task<ICollection<Order>> ApplyPaging(int pageSize, int page = 1)
        {
            var orders = await _db.Orders
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var order in orders)
                order.OrderShawarmas = await GetOrderShawarmas(order.Id);
            
            return orders;
        }

        public async Task<ICollection<Order>> GetList()
        {
            var orders = await _db.Orders
                .OrderBy(o => o.Id)
                .ToListAsync();

            foreach (var order in orders)
                order.OrderShawarmas = await GetOrderShawarmas(order.Id);

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
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetById(int id)
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