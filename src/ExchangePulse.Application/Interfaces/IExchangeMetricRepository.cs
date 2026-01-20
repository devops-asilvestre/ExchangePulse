using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Application.Interfaces
{  
    public interface IExchangeMetricRepository
    {
        Task<IEnumerable<ExchangeMetric>> GetAllAsync();
        Task<ExchangeMetric?> GetByIdAsync(Guid id);
        Task AddAsync(ExchangeMetric metric);
        Task UpdateAsync(ExchangeMetric metric);
        Task DeleteAsync(Guid id);
    }
}
