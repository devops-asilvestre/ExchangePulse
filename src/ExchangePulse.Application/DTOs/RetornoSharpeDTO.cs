namespace ExchangePulse.Application.DTOs.Reports
{
    public class RetornoSharpeDTO
    {
        public DateTime Date { get; set; }
        public decimal LogReturn { get; set; }
        public decimal SharpeDaily { get; set; }
        public decimal SharpeAnnual { get; set; }
    }
}
