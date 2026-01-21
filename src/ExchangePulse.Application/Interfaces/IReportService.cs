using ExchangePulse.Application.DTOs.Reports;

namespace ExchangePulse.Application.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Relatório de Cotações Históricas.
        /// </summary>
        Task<PagedResultDTO<CotacaoHistoricaDTO>> GetCotacoesHistoricasAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório de Médias Móveis (7d, 30d).
        /// </summary>
        Task<PagedResultDTO<MediaMovelDTO>> GetMediasMoveisAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório de Volatilidade (média mensal).
        /// </summary>
        Task<PagedResultDTO<VolatilidadeDTO>> GetVolatilidadeAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório de Retornos e Índice de Sharpe.
        /// </summary>
        Task<PagedResultDTO<RetornoSharpeDTO>> GetRetornosSharpeAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório de Drawdown (mensal).
        /// </summary>
        Task<PagedResultDTO<DrawdownDTO>> GetDrawdownAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório de Value-at-Risk (VaR) 95% (mensal).
        /// </summary>
        Task<PagedResultDTO<VarDTO>> GetValueAtRiskAsync(ReportFilterDTO filter);

        /// <summary>
        /// Relatório Macroeconômico (SELIC, IPCA).
        /// </summary>
        Task<PagedResultDTO<MacroEconomicoDTO>> GetMacroEconomicoAsync(ReportFilterDTO filter);
    }
}
