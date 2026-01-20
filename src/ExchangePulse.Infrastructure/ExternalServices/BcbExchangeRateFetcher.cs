using System.Net.Http;
using System.Text.Json;
using ExchangePulse.Application.Interfaces;

namespace ExchangePulse.Infrastructure.ExternalServices
{
    public class BcbExchangeRateFetcher : IExchangeRateFetcher
    {
        private readonly HttpClient _httpClient;

        public BcbExchangeRateFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<(DateTime date, decimal buy, decimal sell)>> GetUsdBrlRatesAsync(DateTime start, DateTime end)
        {
            string startStr = start.ToString("MM-dd-yyyy");
            string endStr = end.ToString("MM-dd-yyyy");

            string url = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/" +
                         $"CotacaoDolarPeriodo(dataInicial=@dataInicial,dataFinalCotacao=@dataFinalCotacao)" +
                         $"?@dataInicial='{startStr}'&@dataFinalCotacao='{endStr}'&$format=json";

            var response = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);

            var cotacoes = doc.RootElement.GetProperty("value");
            var result = new List<(DateTime, decimal, decimal)>();

            foreach (var item in cotacoes.EnumerateArray())
            {
                var date = DateTime.Parse(item.GetProperty("dataHoraCotacao").ToString()); //.GetDateTime();
                var buy = Decimal.Parse(item.GetProperty("cotacaoCompra").ToString()); //.GetDecimal();
                var sell = Decimal.Parse(item.GetProperty("cotacaoVenda").ToString()); // .GetDecimal();
                result.Add((date, buy, sell));
            }

            return result;
        }
    }
}
