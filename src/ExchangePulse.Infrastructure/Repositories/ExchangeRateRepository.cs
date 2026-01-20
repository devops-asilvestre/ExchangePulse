using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Infrastructure.Persistence;

namespace ExchangePulse.Infrastructure.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangePulseDbContext _context;

        public ExchangeRateRepository(ExchangePulseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllAsync() =>
            await _context.ExchangeRates.AsNoTracking().ToListAsync();

        public async Task<ExchangeRate?> GetByIdAsync(Guid id) =>
            await _context.ExchangeRates.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(ExchangeRate rate)
        {
            if (rate.Id == Guid.Empty)
                rate.Id = Guid.NewGuid();

            _context.ExchangeRates.Add(rate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExchangeRate rate)
        {
            _context.ExchangeRates.Update(rate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var rate = await _context.ExchangeRates.FindAsync(id);
            if (rate != null)
            {
                _context.ExchangeRates.Remove(rate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ExchangeRate>> GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end)
        {
            return await _context.ExchangeRates
                .Where(r => r.CurrencyId == currencyId && r.Date >= start && r.Date <= end)
                .OrderBy(r => r.Date)
                .ToListAsync();
        }
    }
}
