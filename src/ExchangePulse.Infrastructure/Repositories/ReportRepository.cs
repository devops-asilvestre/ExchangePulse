using ExchangePulse.Application.Interfaces;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangePulse.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ExchangePulseDbContext _context;

        public ReportRepository(ExchangePulseDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ExchangeRate> items, int total)> GetExchangeRatesAsync(IEnumerable<Guid> currencyIds, DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection)
        {
            var query = _context.ExchangeRates.AsQueryable()
                .Where(r => r.Date >= start && r.Date <= end);

            if (currencyIds != null && currencyIds.Any())
                query = query.Where(r => currencyIds.Contains(r.CurrencyId));

            query = ApplyOrdering(query, orderBy, orderDirection);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }

        public async Task<(IEnumerable<ExchangeMetric> items, int total)> GetExchangeMetricsAsync(IEnumerable<Guid> currencyIds, DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection)
        {
            var query = _context.ExchangeMetrics.AsQueryable()
                .Where(m => m.Date >= start && m.Date <= end);

            if (currencyIds != null && currencyIds.Any())
                query = query.Where(m => currencyIds.Contains(m.CurrencyId));

            query = ApplyOrdering(query, orderBy, orderDirection);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }

        public async Task<(IEnumerable<ExchangeMetric> items, int total)> GetMacroMetricsAsync(DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection)
        {
            var query = _context.ExchangeMetrics.AsQueryable()
                .Where(m => m.Date >= start && m.Date <= end);

            query = ApplyOrdering(query, orderBy, orderDirection);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string orderBy, string orderDirection)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query;

            var isDesc = string.Equals(orderDirection, "DESC", StringComparison.OrdinalIgnoreCase);

            // Campos comuns: Date, BuyPrice, SellPrice, Volatility30d, SharpeDaily, VaREmpirical95, VaRCornishFisher95
            // Para simplicidade, usar switch por tipos conhecidos
            if (typeof(T) == typeof(ExchangeRate))
            {
                var q = query as IQueryable<ExchangeRate>;
                q = orderBy switch
                {
                    "BuyPrice" => isDesc ? q.OrderByDescending(x => x.BuyPrice) : q.OrderBy(x => x.BuyPrice),
                    "SellPrice" => isDesc ? q.OrderByDescending(x => x.SellPrice) : q.OrderBy(x => x.SellPrice),
                    "Date" => isDesc ? q.OrderByDescending(x => x.Date) : q.OrderBy(x => x.Date),
                    _ => q.OrderBy(x => x.Date)
                };
                return q as IQueryable<T>;
            }

            if (typeof(T) == typeof(ExchangeMetric))
            {
                var q = query as IQueryable<ExchangeMetric>;
                q = orderBy switch
                {
                    "Volatility30d" => isDesc ? q.OrderByDescending(x => x.Volatility30d) : q.OrderBy(x => x.Volatility30d),
                    "SharpeDaily" => isDesc ? q.OrderByDescending(x => x.SharpeDaily) : q.OrderBy(x => x.SharpeDaily),
                    "VaREmpirical95" => isDesc ? q.OrderByDescending(x => x.VaREmpirical95) : q.OrderBy(x => x.VaREmpirical95),
                    "VaRCornishFisher95" => isDesc ? q.OrderByDescending(x => x.VaRCornishFisher95) : q.OrderBy(x => x.VaRCornishFisher95),
                    "Date" => isDesc ? q.OrderByDescending(x => x.Date) : q.OrderBy(x => x.Date),
                    _ => q.OrderBy(x => x.Date)
                };
                return q as IQueryable<T>;
            }

            return query;
        }
    }
}
