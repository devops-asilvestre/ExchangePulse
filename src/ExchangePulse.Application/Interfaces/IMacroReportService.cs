using ExchangePulse.Application.DTOs.Reports;

namespace ExchangePulse.Application.Interfaces
{
    public interface IMacroReportService
    {
        Task<PagedResultDTO<MacroEconomicoDTO>> GetMacroEconomicoAsync(ReportFilterDTO filter);
    }
}
