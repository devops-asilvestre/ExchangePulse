namespace ExchangePulse.Application.DTOs.Reports
{
    public class VarDTO
    {
        public string Periodo { get; set; } // Ex.: "Jan/2026"
        public decimal VaREmpirico95 { get; set; }
        public decimal VaRCornishFisher95 { get; set; }
    }
}
