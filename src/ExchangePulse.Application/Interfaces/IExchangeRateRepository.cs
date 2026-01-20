using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Application.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<IEnumerable<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate?> GetByIdAsync(Guid id);
        Task AddAsync(ExchangeRate rate);
        Task UpdateAsync(ExchangeRate rate);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ExchangeRate>> GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end);
    }
}

