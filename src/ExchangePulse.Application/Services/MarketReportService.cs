using ExchangePulse.Application.DTOs.Reports;
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Application.Aggregators;

namespace ExchangePulse.Application.Services
{
    public class MarketReportService : IMarketReportService
    {
        private readonly IReportRepository _repository;

        public MarketReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDTO<CotacaoHistoricaDTO>> GetCotacoesHistoricasAsync(ReportFilterDTO filter)
        {
            var (rates, total) = await _repository.GetExchangeRatesAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
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

        public async Task<PagedResultDTO<MediaMovelDTO>> GetMediasMoveisAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
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

        public async Task<PagedResultDTO<VolatilidadeDTO>> GetVolatilidadeAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g => new VolatilidadeDTO
                                 {
                                     Periodo = g.Key,
                                     Volatilidade30d = ReportAggregator.ComputeMonthlyVolatility(g)
                                 });

            return new PagedResultDTO<VolatilidadeDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total // opcional: total de grupos
            };
        }

        public async Task<PagedResultDTO<RetornoSharpeDTO>> GetRetornosSharpeAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
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

        public async Task<PagedResultDTO<DrawdownDTO>> GetDrawdownAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g =>
                                 {
                                     var (max, min, dd) = ReportAggregator.ComputeMonthlyDrawdown(g);
                                     return new DrawdownDTO
                                     {
                                         Periodo = g.Key,
                                         Maximo = max,
                                         Minimo = min,
                                         Drawdown = dd
                                     };
                                 });

            return new PagedResultDTO<DrawdownDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total // opcional: total de grupos
            };
        }

        public async Task<PagedResultDTO<VarDTO>> GetValueAtRiskAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetExchangeMetricsAsync(
                filter.CurrencyIds, filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
            );

            var grouped = metrics.GroupBy(m => m.Date.ToString("MMM/yyyy"))
                                 .Select(g =>
                                 {
                                     var (emp, cf) = ReportAggregator.ComputeMonthlyVar(g);
                                     return new VarDTO
                                     {
                                         Periodo = g.Key,
                                         VaREmpirico95 = emp,
                                         VaRCornishFisher95 = cf
                                     };
                                 });

            return new PagedResultDTO<VarDTO>
            {
                Items = grouped,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = total // opcional: total de grupos
            };
        }
    }
}
