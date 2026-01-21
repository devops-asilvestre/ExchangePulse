using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Application.Interfaces
{
    public interface IReportRepository
    {
        Task<(IEnumerable<ExchangeRate> items, int total)> GetExchangeRatesAsync(IEnumerable<Guid> currencyIds, DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection);
        Task<(IEnumerable<ExchangeMetric> items, int total)> GetExchangeMetricsAsync(IEnumerable<Guid> currencyIds, DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection);

        // Macro não depende de currencyId
        Task<(IEnumerable<ExchangeMetric> items, int total)> GetMacroMetricsAsync(DateTime start, DateTime end, int page, int pageSize, string orderBy, string orderDirection);
    }
}
