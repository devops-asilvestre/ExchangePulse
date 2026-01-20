using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ExchangePulse.Application.Services;
using ExchangePulse.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangePulse.Infrastructure.BackgroundServices
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExchangeRateBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        public ExchangeRateBackgroundService(IServiceProvider serviceProvider,
                                             ILogger<ExchangeRateBackgroundService> logger,
                                             IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExchangeRateBackgroundService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var updater = scope.ServiceProvider.GetRequiredService<ExchangeRateUpdater>();
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

                    var currencies = await currencyService.GetAllAsync();

                    foreach (var currency in currencies)
                    {
                        var start = DateTime.UtcNow.AddDays(-1);
                        var end = DateTime.UtcNow;

                        _logger.LogInformation("Buscando cotação para {Currency} ({Code}) entre {Start} e {End}",
                            currency.Name, currency.Code, start, end);

                        await updater.UpdateUsdBrlPeriodAsync(currency.Id, start, end);

                        _logger.LogInformation("Cotação de {Currency} atualizada com sucesso em {DateTime}",
                            currency.Code, DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar cotações.");
                }

                // 🔎 Lê flag do appsettings
                bool isProductionSchedule = _configuration.GetValue<bool>("BackgroundJobs:IsProductionSchedule");

                if (isProductionSchedule)
                {
                    // 🕕 Respeita hora/minuto configurados
                    int updateHour = _configuration.GetValue<int>("BackgroundJobs:ExchangeRateUpdateHour");
                    int updateMinute = _configuration.GetValue<int>("BackgroundJobs:ExchangeRateUpdateMinute");

                    var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

                    var nextRun = now.Date.AddHours(updateHour).AddMinutes(updateMinute);

                    if (now > nextRun)
                        nextRun = nextRun.AddDays(1);

                    var delay = nextRun - now;

                    _logger.LogInformation("Próxima execução agendada para {NextRun}", nextRun);

                    await Task.Delay(delay, stoppingToken);
                }
                else
                {
                    // 🚀 Modo teste: executa a cada 1 minuto
                    _logger.LogInformation("Modo teste ativo. Executando novamente em 1 minuto.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}
