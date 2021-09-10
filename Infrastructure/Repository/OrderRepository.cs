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
        private readonly int _appSettingsConfiguration;
        
        public OrderRepository(ApiContext context, IConfiguration configuration)
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }
        
        public async Task<ICollection<Order>> GetActualOrderList(DateTime date)
        {
            var orders = await _db.Orders.Where(o => o.Date.Day == DateTime.Today.Day).ToListAsync();
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
        
        public async Task<ICollection<Order>> GetOrderList()
        {
            var orders = await _db.Orders.ToListAsync();
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

        public async Task<Order> CreateOrder(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return order;
        }
        
        public async Task<Order> UpdateOrder(Order newOrder)
        {
            var order = await _db.Orders.FindAsync(newOrder.Id);
            
            order.Comment = newOrder.Comment;
            order.Date = newOrder.Date;
            order.IdStatus = newOrder.IdStatus;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return order;
        }
        
        public async Task<Order> DeleteOrder(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            _db.Remove(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id)
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