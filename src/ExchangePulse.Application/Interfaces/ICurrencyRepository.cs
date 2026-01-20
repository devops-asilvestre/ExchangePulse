using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(Guid id);
        Task AddAsync(Currency currency);
        Task UpdateAsync(Currency currency);
        Task DeleteAsync(Guid id);
    }
}
