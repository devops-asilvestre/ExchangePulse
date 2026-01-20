namespace ExchangePulse.Domain.Entities
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }

        // Propriedades calculadas (não mapeadas no banco)
        public decimal Spread => SellPrice - BuyPrice;
        public decimal Average => (BuyPrice + SellPrice) / 2;

        public long Volume { get; set; }
        public string Source { get; set; } = string.Empty;
        public string MacroEvents { get; set; } = string.Empty;
    }
}
