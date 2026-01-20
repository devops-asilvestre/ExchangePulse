using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository;

        public ExchangeRateService(IExchangeRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExchangeRateDTO>> GetAllAsync()
        {
            var rates = await _repository.GetAllAsync();
            return rates.Select(ToDTO);
        }

        public async Task<ExchangeRateDTO?> GetByIdAsync(Guid id)
        {
            var rate = await _repository.GetByIdAsync(id);
            return rate == null ? null : ToDTO(rate);
        }

        public async Task<ExchangeRateDTO> CreateAsync(ExchangeRateDTO dto)
        {
            var entity = FromDTO(dto);
            await _repository.AddAsync(entity);
            return ToDTO(entity);
        }
               

        public async Task<ExchangeRateDTO?> UpdateAsync(Guid id, ExchangeRateDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Date = dto.Date;
            existing.CurrencyId = dto.CurrencyId;
            existing.BuyPrice = dto.BuyPrice;
            existing.SellPrice = dto.SellPrice;
            existing.Volume = dto.Volume;
            existing.Source = dto.Source;
            existing.MacroEvents = dto.MacroEvents;

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

        private static ExchangeRateDTO ToDTO(ExchangeRate r) =>
            new ExchangeRateDTO
            {
                Id = r.Id,
                Date = r.Date,
                CurrencyId = r.CurrencyId,
                BuyPrice = r.BuyPrice,
                SellPrice = r.SellPrice,
                Average = r.Average,
                Volume = r.Volume,
                Source = r.Source,
                MacroEvents = r.MacroEvents
            };

        private static ExchangeRate FromDTO(ExchangeRateDTO dto) =>
            new ExchangeRate
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Date = dto.Date,
                CurrencyId = dto.CurrencyId,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                Volume = dto.Volume,
                Source = dto.Source,
                MacroEvents = dto.MacroEvents
            };

        public async Task<IEnumerable<ExchangeRateDTO>> GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end)
        {
            var entities = await _repository.GetByCurrencyAsync(currencyId, start, end);

            return entities.Select(r => new ExchangeRateDTO
            {
                Date = r.Date,
                CurrencyId = r.CurrencyId,
                BuyPrice = r.BuyPrice,
                SellPrice = r.SellPrice,
                Average = r.Average,
                Volume = r.Volume,
                Source = r.Source,
                MacroEvents = r.MacroEvents
            });
        }
    }
}
