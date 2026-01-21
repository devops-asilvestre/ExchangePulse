namespace ExchangePulse.Application.DTOs.Reports
{
    public class CotacaoHistoricaDTO
    {
        public DateTime Date { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Average { get; set; }
    }
}
