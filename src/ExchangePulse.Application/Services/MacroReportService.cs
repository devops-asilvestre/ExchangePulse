using ExchangePulse.Application.DTOs.Reports;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class MacroReportService : IMacroReportService
    {
        private readonly IReportRepository _repository;

        public MacroReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDTO<MacroEconomicoDTO>> GetMacroEconomicoAsync(ReportFilterDTO filter)
        {
            var (metrics, total) = await _repository.GetMacroMetricsAsync(
                filter.Start, filter.End,
                filter.Page, filter.PageSize, filter.OrderBy, filter.OrderDirection
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
