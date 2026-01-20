namespace ExchangePulse.Domain.Entities
{
    public class ExchangeMetric
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public decimal DailyVariation { get; set; }
        public decimal LogReturn { get; set; }
        public decimal MovingAverage7d { get; set; }
        public decimal MovingAverage30d { get; set; }
        public decimal Volatility30d { get; set; }

        public decimal SharpeDaily { get; set; }
        public decimal SharpeAnnual { get; set; }
        public decimal Drawdown { get; set; }
        public decimal Beta { get; set; }
        public decimal VaREmpirical95 { get; set; }
        public decimal VaRCornishFisher95 { get; set; }

        public decimal InterestRate { get; set; }
        public decimal Inflation { get; set; }
    }
}
