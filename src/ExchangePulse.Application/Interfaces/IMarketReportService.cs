using ExchangePulse.Application.DTOs.Reports;

namespace ExchangePulse.Application.Interfaces
{
    public interface IMarketReportService
    {
        Task<PagedResultDTO<CotacaoHistoricaDTO>> GetCotacoesHistoricasAsync(ReportFilterDTO filter);
        Task<PagedResultDTO<MediaMovelDTO>> GetMediasMoveisAsync(ReportFilterDTO filter);
        Task<PagedResultDTO<VolatilidadeDTO>> GetVolatilidadeAsync(ReportFilterDTO filter);
        Task<PagedResultDTO<RetornoSharpeDTO>> GetRetornosSharpeAsync(ReportFilterDTO filter);
        Task<PagedResultDTO<DrawdownDTO>> GetDrawdownAsync(ReportFilterDTO filter);
        Task<PagedResultDTO<VarDTO>> GetValueAtRiskAsync(ReportFilterDTO filter);
    }
}
