using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangePulse.Domain.Entities;
using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class ExchangeMetricService : IExchangeMetricService
    {
        private readonly IExchangeMetricRepository _repository;

        public ExchangeMetricService(IExchangeMetricRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExchangeMetricDTO>> GetAllAsync()
        {
            var metrics = await _repository.GetAllAsync();
            return metrics.Select(ToDTO);
        }

        public async Task<ExchangeMetricDTO?> GetByIdAsync(Guid id)
        {
            var metric = await _repository.GetByIdAsync(id);
            return metric == null ? null : ToDTO(metric);
        }

        public async Task<ExchangeMetricDTO> CreateAsync(ExchangeMetricDTO dto)
        {
            var entity = FromDTO(dto);
            await _repository.AddAsync(entity);
            return ToDTO(entity);
        }

        public async Task<ExchangeMetricDTO?> UpdateAsync(Guid id, ExchangeMetricDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Date = dto.Date;
            existing.CurrencyId = dto.CurrencyId;
            existing.DailyVariation = dto.DailyVariation;
            existing.LogReturn = dto.LogReturn;
            existing.MovingAverage7d = dto.MovingAverage7d;
            existing.MovingAverage30d = dto.MovingAverage30d;
            existing.Volatility30d = dto.Volatility30d;
            existing.SharpeDaily = dto.SharpeDaily;
            existing.SharpeAnnual = dto.SharpeAnnual;
            existing.Drawdown = dto.Drawdown;
            existing.Beta = dto.Beta;
            existing.VaREmpirical95 = dto.VaREmpirical95;
            existing.VaRCornishFisher95 = dto.VaRCornishFisher95;
            existing.InterestRate = dto.InterestRate;
            existing.Inflation = dto.Inflation;

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

        private static ExchangeMetricDTO ToDTO(ExchangeMetric m) =>
            new ExchangeMetricDTO
            {
                Id = m.Id,
                Date = m.Date,
                CurrencyId = m.CurrencyId,
                DailyVariation = m.DailyVariation,
                LogReturn = m.LogReturn,
                MovingAverage7d = m.MovingAverage7d,
                MovingAverage30d = m.MovingAverage30d,
                Volatility30d = m.Volatility30d,
                SharpeDaily = m.SharpeDaily,
                SharpeAnnual = m.SharpeAnnual,
                Drawdown = m.Drawdown,
                Beta = m.Beta,
                VaREmpirical95 = m.VaREmpirical95,
                VaRCornishFisher95 = m.VaRCornishFisher95,
                InterestRate = m.InterestRate,
                Inflation = m.Inflation
            };

        private static ExchangeMetric FromDTO(ExchangeMetricDTO dto) =>
            new ExchangeMetric
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Date = dto.Date,
                CurrencyId = dto.CurrencyId,
                DailyVariation = dto.DailyVariation,
                LogReturn = dto.LogReturn,
                MovingAverage7d = dto.MovingAverage7d,
                MovingAverage30d = dto.MovingAverage30d,
                Volatility30d = dto.Volatility30d,
                SharpeDaily = dto.SharpeDaily,
                SharpeAnnual = dto.SharpeAnnual,
                Drawdown = dto.Drawdown,
                Beta = dto.Beta,
                VaREmpirical95 = dto.VaREmpirical95,
                VaRCornishFisher95 = dto.VaRCornishFisher95,
                InterestRate = dto.InterestRate,
                Inflation = dto.Inflation
            };
    }
}
