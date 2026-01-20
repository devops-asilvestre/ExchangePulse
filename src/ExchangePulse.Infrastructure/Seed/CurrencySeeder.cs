using System.Collections.Generic;
using System.Linq;
using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Infrastructure.Persistence.Seed
{
    public static class CurrencySeeder
    {
        public static void Seed(ExchangePulseDbContext context)
        {
            if (!context.Currencies.Any())
            {
                var currencies = new List<Currency>
                {
                    new Currency { Code = "USD", Name = "United States Dollar",     Country = "United States" },
                    new Currency { Code = "BRL", Name = "Brazilian Real",           Country = "Brazil" },
                    new Currency { Code = "PYG", Name = "Paraguayan Guarani",       Country = "Paraguay" },
                    new Currency { Code = "EUR", Name = "Euro",                     Country = "European Union" },
                    new Currency { Code = "JPY", Name = "Japanese Yen",             Country = "Japan" },
                    new Currency { Code = "GBP", Name = "British Pound Sterling",   Country = "United Kingdom" },
                    new Currency { Code = "ARS", Name = "Argentine Peso",           Country = "Argentina" },
                    new Currency { Code = "CLP", Name = "Chilean Peso",             Country = "Chile" },
                    new Currency { Code = "MXN", Name = "Mexican Peso",             Country = "Mexico" },
                    new Currency { Code = "CNY", Name = "Chinese Yuan Renminbi",    Country = "China" }
                };

                context.Currencies.AddRange(currencies);
                context.SaveChanges();
            }
        }
    }
}
