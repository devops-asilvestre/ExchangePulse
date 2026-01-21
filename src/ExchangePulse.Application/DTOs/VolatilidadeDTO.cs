namespace ExchangePulse.Application.DTOs.Reports
{
    public class VolatilidadeDTO
    {
        public string Periodo { get; set; } // Ex.: "Jan/2026"
        public decimal Volatilidade30d { get; set; }
    }
}
