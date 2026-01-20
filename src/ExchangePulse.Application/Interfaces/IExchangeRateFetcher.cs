using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangePulse.Application.Interfaces
{
    public interface IExchangeRateFetcher
    {
        Task<List<(DateTime date, decimal buy, decimal sell)>> GetUsdBrlRatesAsync(DateTime start, DateTime end);
    }
}
