using Microsoft.AspNetCore.Mvc;
using ExchangePulse.Application.Services;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Api.Controllers
{
    /// <summary>
    /// Controller responsável por executar atualizações manuais de cotações e métricas.
    /// Permite ao usuário informar uma data ou período para reprocessar dados em caso de falha.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExchangeRateUpdateController : ControllerBase
    {
        private readonly ExchangeRateUpdater _updater;
        private readonly ICurrencyService _currencyService;

        public ExchangeRateUpdateController(ExchangeRateUpdater updater, ICurrencyService currencyService)
        {
            _updater = updater;
            _currencyService = currencyService;
        }

        /// <summary>
        /// Executa atualização manual para uma moeda específica.
        /// </summary>
        [HttpPost("manual")]
        public async Task<IActionResult> ManualUpdate(Guid currencyId, DateTime start, DateTime end)
        {
            await _updater.UpdateUsdBrlPeriodAsync(currencyId, start, end);
            return Ok(new
            {
                message = "Atualização manual concluída com sucesso.",
                currencyId,
                start,
                end
            });
        }

        /// <summary>
        /// Executa atualização manual para todas as moedas cadastradas em um período informado.
        /// </summary>
        [HttpPost("manual/all")]
        public async Task<IActionResult> ManualUpdateAll(DateTime start, DateTime end)
        {
            var currencies = await _currencyService.GetAllAsync();

            foreach (var currency in currencies)
            {
                await _updater.UpdateUsdBrlPeriodAsync(currency.Id, start, end);
            }

            return Ok(new
            {
                message = "Atualização manual concluída para todas as moedas.",
                start,
                end,
                totalCurrencies = currencies.Count()
            });
        }
    }
}
