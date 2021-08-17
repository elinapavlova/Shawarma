using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Models.OrderShawarma;

namespace Infrastructure.Repository
{
    public class OrderShawarmaRepository : IOrderShawarmaRepository
    {
        private readonly ApiContext _db;

        public OrderShawarmaRepository(ApiContext context)
        {
            _db = context;
        }

        public async Task<ICollection<OrderShawarma>> GetOrderShawarmaList()
        {
            var ordershawas = await _db.OrderShawarmas.ToListAsync();
            return ordershawas;
        }

        public async void CreateOrderShawarma(OrderShawarma orderShawarma)
        {
            await _db.OrderShawarmas.AddAsync(orderShawarma);
            await _db.SaveChangesAsync();
        }

        public void UpdateOrderShawarma(OrderShawarma newOrderShawa)
        {
            var orderShawa = _db.OrderShawarmas.FindAsync(newOrderShawa.Id).Result;
            
            orderShawa.Number = newOrderShawa.Number;

            _db.OrderShawarmas.Update(orderShawa);
            _db.SaveChangesAsync();
        }

        public void DeleteOrderShawarma(int id)
        {
            var orderShawa = _db.OrderShawarmas.FindAsync(id).Result;
            _db.OrderShawarmas.Remove(orderShawa);
            _db.SaveChangesAsync();
        }

        public async Task<OrderShawarma> GetOrderShawarmaById(int id)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(id);
            return orderShawa;
        }
    }
}