using ExchangePulse.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

public class BcbDataFetcher : IBcbDataFetcher
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public BcbDataFetcher(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<decimal> GetSelicAsync(DateTime date)
    {
        int selicCode = _config.GetValue<int>("BcbSgs:SelicSeriesCode", 11);
        var data = await FetchSeriesAsync(selicCode, date, date);
        return ParseLastValueOrZero(data);
    }

    public async Task<decimal> GetIpcaAsync(DateTime date)
    {
        int ipcaCode = _config.GetValue<int>("BcbSgs:IpcaSeriesCode", 10844);
        int maxFallback = _config.GetValue<int>("BcbSgs:IpcaMaxFallbackMonths", 3);

        // mês alvo
        var firstDay = new DateTime(date.Year, date.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        // tenta mês alvo; se vazio, retrocede até maxFallback meses
        for (int i = 0; i <= maxFallback; i++)
        {
            var start = firstDay.AddMonths(-i);
            var end = lastDay.AddMonths(-i);

            var data = await FetchSeriesAsync(ipcaCode, start, end);
            var value = ParseLastValueOrZero(data);
            if (value != 0) return value;
        }

        return 0; // nada divulgado nos últimos N meses
    }

    private async Task<List<BcbSerieDto>?> FetchSeriesAsync(int seriesCode, DateTime start, DateTime end)
    {
        string url =
            $"https://api.bcb.gov.br/dados/serie/bcdata.sgs.{seriesCode}/dados?formato=json&dataInicial={start:dd/MM/yyyy}&dataFinal={end:dd/MM/yyyy}";
        try
        {
            return await _httpClient.GetFromJsonAsync<List<BcbSerieDto>>(url);
        }
        catch
        {
            return null;
        }
    }

    private static decimal ParseLastValueOrZero(List<BcbSerieDto>? data)
    {
        if (data == null || data.Count == 0) return 0;
        var last = data.Last().Valor;
        return decimal.TryParse(last, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture,
                                out var v)
            ? v
            : 0;
    }

    private class BcbSerieDto
    {
        public string Data { get; set; } = default!;
        public string Valor { get; set; } = default!;
    }
}
