using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Infrastructure.Persistence;

namespace ExchangePulse.Infrastructure.Repositories
{
    public class ExchangeMetricRepository : IExchangeMetricRepository
    {
        private readonly ExchangePulseDbContext _context;

        public ExchangeMetricRepository(ExchangePulseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExchangeMetric>> GetAllAsync() =>
            await _context.ExchangeMetrics.AsNoTracking().ToListAsync();

        public async Task<ExchangeMetric?> GetByIdAsync(Guid id) =>
            await _context.ExchangeMetrics.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

        public async Task AddAsync(ExchangeMetric metric)
        {
            if (metric.Id == Guid.Empty)
                metric.Id = Guid.NewGuid();

            _context.ExchangeMetrics.Add(metric);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExchangeMetric metric)
        {
            _context.ExchangeMetrics.Update(metric);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var metric = await _context.ExchangeMetrics.FindAsync(id);
            if (metric != null)
            {
                _context.ExchangeMetrics.Remove(metric);
                await _context.SaveChangesAsync();
            }
        }
    }
}
