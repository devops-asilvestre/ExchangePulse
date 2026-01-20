using ExchangePulse.Application.Interfaces;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangePulse.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ExchangePulseDbContext _context;

        public CurrencyRepository(ExchangePulseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _context.Currencies.AsNoTracking().ToListAsync();
        }

        public async Task<Currency?> GetByIdAsync(Guid id)
        {
            return await _context.Currencies.AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Currency currency)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Currency currency)
        {
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
                await _context.SaveChangesAsync();
            }
        }
    }
}
