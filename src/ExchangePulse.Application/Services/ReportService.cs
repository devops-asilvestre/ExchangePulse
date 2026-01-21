using ExchangePulse.Application.DTOs.Reports;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        // 1. Relatório de Cotações Históricas
        public async Task<PagedResultDTO<CotacaoHistoricaDTO>> GetCotacoesHistoricasAsync(ReportFilterDTO filter)
        {
            var (rates, total) = await _repository.GetExchangeRatesAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var items = rates.Select(r => new CotacaoHistoricaDTO
            {
                Date = r.Date,
                BuyPrice = r.BuyPrice,
                SellPrice = r.SellPrice,
                Average = (r.BuyPrice + r.SellPrice) / 2
            });

            return new PagedResultDTO<CotacaoHistoricaDTO>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 2. Relatório de Médias Móveis
        public async Task<PagedResultDTO<MediaMovelDTO>> GetMediasMoveisAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var items = metrics.Select(m => new MediaMovelDTO
            {
                Date = m.Date,
                MovingAverage7d = m.MovingAverage7d,
                MovingAverage30d = m.MovingAverage30d
            });

            return new PagedResultDTO<MediaMovelDTO>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 3. Relatório de Volatilidade
        public async Task<PagedResultDTO<VolatilidadeDTO>> GetVolatilidadeAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g => new VolatilidadeDTO
                                 {
                                     Periodo = g.Key,
                                     Volatilidade30d = g.Average(x => x.Volatility30d)
                                 });

            return new PagedResultDTO<VolatilidadeDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 4. Relatório de Retornos e Índice de Sharpe
        public async Task<PagedResultDTO<RetornoSharpeDTO>> GetRetornosSharpeAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var items = metrics.Select(m => new RetornoSharpeDTO
            {
                Date = m.Date,
                LogReturn = m.LogReturn,
                SharpeDaily = m.SharpeDaily,
                SharpeAnnual = m.SharpeAnnual
            });

            return new PagedResultDTO<RetornoSharpeDTO>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 5. Relatório de Drawdown
        public async Task<PagedResultDTO<DrawdownDTO>> GetDrawdownAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g => new DrawdownDTO
                                 {
                                     Periodo = g.Key,
                                     Maximo = g.Max(x => x.Drawdown), // Ajustar conforme regra de negócio
                                     Minimo = g.Min(x => x.Drawdown),
                                     Drawdown = g.Average(x => x.Drawdown)
                                 });

            return new PagedResultDTO<DrawdownDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 6. Relatório de Value-at-Risk (VaR)
        public async Task<PagedResultDTO<VarDTO>> GetValueAtRiskAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g => new VarDTO
                                 {
                                     Periodo = g.Key,
                                     VaREmpirico95 = g.Average(x => x.VaREmpirical95),
                                     VaRCornishFisher95 = g.Average(x => x.VaRCornishFisher95)
                                 });

            return new PagedResultDTO<VarDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }

        // 7. Relatório Macroeconômico
        public async Task<PagedResultDTO<MacroEconomicoDTO>> GetMacroEconomicoAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetMacroMetricsAsync(
                filter.Start, filter.End,
                filter.Page, filter.PageSize,
                filter.OrderBy, filter.OrderDirection
            );

            var items = metrics.Select(m => new MacroEconomicoDTO
            {
                Date = m.Date,
                Selic = m.InterestRate,
                Ipca = m.Inflation
            });

            return new PagedResultDTO<MacroEconomicoDTO>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total
            };
        }
    }
}
