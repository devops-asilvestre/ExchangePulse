namespace ExchangePulse.Domain.Entities
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty; // Ex: "BRL", "USD", "PYG"
        public string Name { get; set; } = string.Empty; // Ex: "Real Brasileiro"
        public string Country { get; set; } = string.Empty; // Ex: "Brasil"
    }
}
