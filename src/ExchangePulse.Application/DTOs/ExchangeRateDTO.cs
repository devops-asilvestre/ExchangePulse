namespace ExchangePulse.Application.DTOs
{
    public class ExchangeRateDTO
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Average { get; set; }
        public long Volume { get; set; }
        public string Source { get; set; } = string.Empty;
        public string MacroEvents { get; set; } = string.Empty;
    }
}
