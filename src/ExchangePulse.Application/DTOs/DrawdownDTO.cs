namespace ExchangePulse.Application.DTOs.Reports
{
    public class DrawdownDTO
    {
        public string Periodo { get; set; } // Ex.: "Jan/2026"
        public decimal Maximo { get; set; }
        public decimal Minimo { get; set; }
        public decimal Drawdown { get; set; }
    }
}
