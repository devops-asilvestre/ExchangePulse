using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CurrencyDTO>> GetAllAsync()
        {
            var currencies = await _repository.GetAllAsync();
            return currencies.Select(ToDTO);
        }

        public async Task<CurrencyDTO?> GetByIdAsync(Guid id)
        {
            var currency = await _repository.GetByIdAsync(id);
            return currency == null ? null : ToDTO(currency);
        }

        public async Task<CurrencyDTO> CreateAsync(CurrencyDTO dto)
        {
            var entity = FromDTO(dto);
            await _repository.AddAsync(entity);
            return ToDTO(entity);
        }

        public async Task<CurrencyDTO?> UpdateAsync(Guid id, CurrencyDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Code = dto.Code;
            existing.Name = dto.Name;
            existing.Country = dto.Country;

            await _repository.UpdateAsync(existing);
            return ToDTO(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        // Conversão manual
        private static CurrencyDTO ToDTO(Currency c) =>
            new CurrencyDTO { Id = c.Id, Code = c.Code, Name = c.Name, Country = c.Country };

        private static Currency FromDTO(CurrencyDTO dto) =>
            new Currency { Id = dto.Id, Code = dto.Code, Name = dto.Name, Country = dto.Country };
    }
}
