using Microsoft.AspNetCore.Mvc;
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Application.DTOs.Reports;

namespace ExchangePulse.Api.Controllers
{
    /// <summary>
    /// Controller de relatórios financeiros e macroeconômicos.
    /// Finalidade: expor endpoints para consulta de relatórios com filtros globais,
    /// paginação, ordenação e suporte a múltiplos CurrencyIds.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Relatório de Cotações Históricas.
        /// </summary>
        /// <param name="filter">
        /// Parâmetros:
        /// - CurrencyIds: array de GUIDs de moedas.
        /// - Start/End: período (yyyy-MM-dd).
        /// - Page/PageSize: paginação.
        /// - OrderBy/OrderDirection: ordenação (ex.: Date, BuyPrice | ASC/DESC).
        /// </param>
        [HttpPost("cotacoes")]
        public async Task<IActionResult> GetCotacoesHistoricas([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetCotacoesHistoricasAsync(filter));

        /// <summary>
        /// Relatório de Médias Móveis (7d, 30d).
        /// </summary>
        [HttpPost("medias-moveis")]
        public async Task<IActionResult> GetMediasMoveis([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetMediasMoveisAsync(filter));

        /// <summary>
        /// Relatório de Volatilidade (média mensal).
        /// </summary>
        [HttpPost("volatilidade")]
        public async Task<IActionResult> GetVolatilidade([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetVolatilidadeAsync(filter));

        /// <summary>
        /// Relatório de Retornos e Índice de Sharpe.
        /// </summary>
        [HttpPost("retornos-sharpe")]
        public async Task<IActionResult> GetRetornosSharpe([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetRetornosSharpeAsync(filter));

        /// <summary>
        /// Relatório de Drawdown (mensal).
        /// </summary>
        [HttpPost("drawdown")]
        public async Task<IActionResult> GetDrawdown([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetDrawdownAsync(filter));

        /// <summary>
        /// Relatório de Value-at-Risk (VaR) 95% (mensal).
        /// </summary>
        [HttpPost("var")]
        public async Task<IActionResult> GetValueAtRisk([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetValueAtRiskAsync(filter));

        /// <summary>
        /// Relatório Macroeconômico (SELIC, IPCA).
        /// </summary>
        /// <remarks>
        /// CurrencyIds é ignorado neste relatório.
        /// </remarks>
        [HttpPost("macro")]
        public async Task<IActionResult> GetMacroEconomico([FromBody] ReportFilterDTO filter)
            => Ok(await _reportService.GetMacroEconomicoAsync(filter));
    }
}
