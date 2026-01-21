using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Application.Services
{
    public class ExchangeRateUpdater
    {
        private readonly IExchangeRateService _rateService;
        private readonly IExchangeMetricService _metricService;
        private readonly IExchangeRateFetcher _fetcher;
        private readonly IBcbDataFetcher _bcbDataFetcher;

        public ExchangeRateUpdater(IExchangeRateService rateService,
                                   IExchangeMetricService metricService,
                                   IExchangeRateFetcher fetcher,
                                   IBcbDataFetcher bcbDataFetcher)
        {
            _rateService = rateService;
            _metricService = metricService;
            _fetcher = fetcher;
            _bcbDataFetcher = bcbDataFetcher;
        }

        public async Task UpdateUsdBrlPeriodAsync(Guid currencyId, DateTime start, DateTime end)
        {
            var rates = await _fetcher.GetUsdBrlRatesAsync(start, end);

            foreach (var (date, buy, sell) in rates)
            {
                // 1. Salva cotação
                var rateDto = new ExchangeRateDTO
                {
                    Date = date,
                    CurrencyId = currencyId,
                    BuyPrice = buy,
                    SellPrice = sell,
                    Average = (buy + sell) / 2,
                    Volume = 0,
                    Source = "Banco Central do Brasil",
                    MacroEvents = "Cotação PTAX diária"
                };
                await _rateService.CreateAsync(rateDto);

                // 2. Busca histórico dos últimos 30 dias
                var history = await _rateService.GetByCurrencyAsync(currencyId, date.AddDays(-30), date);
                var ordered = history.OrderBy(r => r.Date).ToList();
                var prices = ordered.Select(r => r.Average).ToList();

                // Retornos logarítmicos
                var returns = new List<double>();
                for (int i = 1; i < prices.Count; i++)
                    returns.Add(Math.Log((double)(prices[i] / prices[i - 1])));

                // 3. Calcula métricas estatísticas
                decimal movingAvg7d = prices.Count >= 7 ? prices.TakeLast(7).Average() : 0;
                decimal movingAvg30d = prices.Count >= 30 ? prices.TakeLast(30).Average() : 0;

                decimal volatility30d = returns.Count >= 30
                    ? (decimal)Math.Sqrt(Variance(returns.TakeLast(30)))
                    : 0;

                decimal sharpeDaily = returns.Any() ? (decimal)Math.Sqrt(Variance(returns))  > 0 ?   (decimal)(returns.Average() - 0.0001) / (decimal)Math.Sqrt(Variance(returns)) : 0:0 ;

                decimal sharpeAnnual = sharpeDaily * (decimal)Math.Sqrt(252);

                decimal drawdown = prices.Any()
                    ? (prices.Min() - prices.Max()) / prices.Max()
                    : 0;

                decimal varEmpirical95 = returns.Any()
                    ? (decimal)returns.OrderBy(r => r).ElementAt((int)(returns.Count * 0.05))
                    : 0;

                // 4. Busca dados macroeconômicos via API BCB
                var selic = await _bcbDataFetcher.GetSelicAsync(date);
                var ipca = await _bcbDataFetcher.GetIpcaAsync(date);

                // 5. Cria métricas
                var metricDto = new ExchangeMetricDTO
                {
                    Date = date,
                    CurrencyId = currencyId,
                    DailyVariation = (sell - buy) / buy,
                    LogReturn = (decimal)Math.Log((double)(sell / buy)),
                    MovingAverage7d = movingAvg7d,
                    MovingAverage30d = movingAvg30d,
                    Volatility30d = volatility30d,
                    SharpeDaily = sharpeDaily,
                    SharpeAnnual = sharpeAnnual,
                    Drawdown = drawdown,
                    Beta = 0, // precisa de benchmark externo
                    VaREmpirical95 = varEmpirical95,
                    VaRCornishFisher95 = 0, // precisa calcular skew/kurtosis
                    InterestRate = selic,
                    Inflation = ipca
                };

                
                await _metricService.CreateAsync(metricDto);
            }
        }
        
        // Função auxiliar para variância
        private static double Variance(IEnumerable<double> values)
        {
            var list = values.ToList();
            if (!list.Any()) return 0;

            double avg = list.Average();
            double sumSq = list.Sum(v => Math.Pow(v - avg, 2));
            return sumSq / list.Count;
        }
    }
}
