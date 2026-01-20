using ExchangePulse.Application.DTOs;

namespace ExchangePulse.Application.Interfaces
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRateDTO>> GetAllAsync();
        Task<ExchangeRateDTO?> GetByIdAsync(Guid id);
        Task<ExchangeRateDTO> CreateAsync(ExchangeRateDTO dto);
        
        Task<ExchangeRateDTO?> UpdateAsync(Guid id, ExchangeRateDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ExchangeRateDTO>> GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end); 
    }
}
