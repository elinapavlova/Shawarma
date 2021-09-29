using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.OrderShawarma;

namespace Infrastructure.Repository
{
    public class OrderShawarmaRepository : IOrderShawarmaRepository
    {
        private readonly ApiContext _db;
        private readonly int _pageSize;
        public OrderShawarmaRepository
        (
            ApiContext context,
             IConfiguration configuration
        )
        {
            _db = context;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<ICollection<OrderShawarma>> GetList()
        {
            var orderShawarmas = await _db.OrderShawarmas
                .OrderBy(o => o.Id)
                .ToListAsync();
            return orderShawarmas;
        }

        public async Task<int> Count()
        {
            var count = await _db.OrderShawarmas.CountAsync();
            return count;
        }

        public async Task<ICollection<OrderShawarma>> GetPage(int pageSize, int page = 1)
        {
            var orderShawarmas = await _db.OrderShawarmas
                .OrderBy(o => o.Id)
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();
            return orderShawarmas;
        }

        public async Task<OrderShawarma> Create(OrderShawarma orderShawarma)
        {
            await _db.OrderShawarmas.AddAsync(orderShawarma);
            await _db.SaveChangesAsync();

            return orderShawarma;
        }

        public async Task<OrderShawarma> Edit(OrderShawarma newOrderShawarma)
        {
            var orderShawarma = await _db.OrderShawarmas.FindAsync(newOrderShawarma.Id);
            orderShawarma.Number = newOrderShawarma.Number;

            _db.OrderShawarmas.Update(orderShawarma);
            await _db.SaveChangesAsync();

            return orderShawarma;
        }

        public async Task<OrderShawarma> Delete(int id)
        {
            var orderShawarma = await _db.OrderShawarmas.FindAsync(id);
            _db.OrderShawarmas.Remove(orderShawarma);
            await _db.SaveChangesAsync();

            return orderShawarma;
        }

        public async Task<OrderShawarma> GetById(int id)
        {
            var orderShawarma = await _db.OrderShawarmas.FindAsync(id);
            return orderShawarma;
        }
    }
}