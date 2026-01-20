using ExchangePulse.Application.DTOs;

namespace ExchangePulse.Application.Interfaces
{
    public interface IExchangeMetricService
    {
        Task<IEnumerable<ExchangeMetricDTO>> GetAllAsync();
        Task<ExchangeMetricDTO?> GetByIdAsync(Guid id);
        Task<ExchangeMetricDTO> CreateAsync(ExchangeMetricDTO dto);
        Task<ExchangeMetricDTO?> UpdateAsync(Guid id, ExchangeMetricDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
