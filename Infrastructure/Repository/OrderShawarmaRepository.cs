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

        public async Task<OrderShawarma> CreateOrderShawarma(OrderShawarma orderShawarma)
        {
            await _db.OrderShawarmas.AddAsync(orderShawarma);
            await _db.SaveChangesAsync();

            return orderShawarma;
        }

        public async Task<OrderShawarma> UpdateOrderShawarma(OrderShawarma newOrderShawa)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(newOrderShawa.Id);
            
            orderShawa.Number = newOrderShawa.Number;

            _db.OrderShawarmas.Update(orderShawa);
            await _db.SaveChangesAsync();

            return orderShawa;
        }

        public async Task<OrderShawarma> DeleteOrderShawarma(int id)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(id);
            _db.OrderShawarmas.Remove(orderShawa);
            await _db.SaveChangesAsync();

            return orderShawa;
        }

        public async Task<OrderShawarma> GetOrderShawarmaById(int id)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(id);
            return orderShawa;
        }
    }
}