using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Order;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiContext _db;

        public OrderRepository(ApiContext context)
        {
            _db = context;
        }
        
        public async Task<ICollection<Order>> GetOrderList()
        {
            var orders = await _db.Orders.ToListAsync();

            foreach (var order in orders)
            {
                var orderShawas = 
                    await _db.OrderShawarmas.Where(o => o.OrderId == order.Id).ToListAsync();
                
                order.OrderShawarmas = orderShawas;
            }
            
            return orders;
        }

        public async void CreateOrder(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
        }
        
        public void UpdateOrder(Order newOrder)
        {
            var order = _db.Orders.FindAsync(newOrder.Id).Result;
            
            order.Comment = newOrder.Comment;
            order.Date = newOrder.Date;
            order.IdStatus = newOrder.IdStatus;

            _db.Orders.Update(order);
            _db.SaveChangesAsync();
        }
        
        public void DeleteOrder(int id)
        {
            var order = _db.Orders.FindAsync(id).Result;
            _db.Remove(order);
            _db.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
                return null;
            
            var orderShawas = await _db.Orders.ToListAsync();
            if (orderShawas.Count == 0)
                return order;
            
            var orderShawasOfUser = 
                    await _db.OrderShawarmas.Where(o => o.OrderId == order.Id).ToListAsync();
            if (orderShawasOfUser.Count == 0)
                return order;

            order.OrderShawarmas = orderShawasOfUser;
            
            return order;
        }
    }
}