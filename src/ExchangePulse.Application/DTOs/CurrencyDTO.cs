namespace ExchangePulse.Application.DTOs
{
    public class CurrencyDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;   // Ex: "USD"
        public string Name { get; set; } = string.Empty;   // Ex: "United States Dollar"
        public string Country { get; set; } = string.Empty; // Ex: "United States"
    }
}
