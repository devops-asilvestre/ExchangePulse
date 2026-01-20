using ExchangePulse.Application.DTOs;

namespace ExchangePulse.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDTO>> GetAllAsync();
        Task<CurrencyDTO?> GetByIdAsync(Guid id);
        Task<CurrencyDTO> CreateAsync(CurrencyDTO dto);
        Task<CurrencyDTO?> UpdateAsync(Guid id, CurrencyDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
