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
        private readonly int _appSettingsConfiguration;
        public OrderShawarmaRepository
        (
            ApiContext context,
             IConfiguration configuration
        )
        {
            _db = context;
            _appSettingsConfiguration = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
        }

        public async Task<ICollection<OrderShawarma>> ApplyPaging(ICollection<OrderShawarma> source, int pageSize, int page = 1)
        {
            var shawarmas = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return shawarmas;
        }

        public async Task<ICollection<OrderShawarma>> GetList()
        {
            var ordershawas = await _db.OrderShawarmas
                .OrderBy(o => o.Id)
                .ToListAsync();
            return ordershawas;
        }

        public async Task<int> Count()
        {
            var count = await _db.OrderShawarmas.CountAsync();
            return count;
        }

        public async Task<ICollection<OrderShawarma>> GetPage(int pageSize, int page = 1)
        {
            var source = await GetList();
            var result = await ApplyPaging(source, _appSettingsConfiguration, page);
            return result;
        }

        public async Task<OrderShawarma> Create(OrderShawarma orderShawarma)
        {
            await _db.OrderShawarmas.AddAsync(orderShawarma);
            await _db.SaveChangesAsync();

            return orderShawarma;
        }

        public async Task<OrderShawarma> Edit(OrderShawarma newOrderShawa)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(newOrderShawa.Id);
            
            orderShawa.Number = newOrderShawa.Number;

            _db.OrderShawarmas.Update(orderShawa);
            await _db.SaveChangesAsync();

            return orderShawa;
        }

        public async Task<OrderShawarma> Delete(int id)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(id);
            _db.OrderShawarmas.Remove(orderShawa);
            await _db.SaveChangesAsync();

            return orderShawa;
        }

        public async Task<OrderShawarma> GetById(int id)
        {
            var orderShawa = await _db.OrderShawarmas.FindAsync(id);
            return orderShawa;
        }
    }
}