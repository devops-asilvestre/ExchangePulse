namespace ExchangePulse.Application.DTOs.Reports
{
    public class MediaMovelDTO
    {
        public DateTime Date { get; set; }
        public decimal MovingAverage7d { get; set; }
        public decimal MovingAverage30d { get; set; }
    }
}
