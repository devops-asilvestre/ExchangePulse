namespace ExchangePulse.Application.DTOs.Reports
{
    public class ReportFilterDTO
    {
        public IEnumerable<Guid> CurrencyIds { get; set; } = new List<Guid>();
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public string OrderBy { get; set; } = "Date"; // Ex.: "Date", "BuyPrice", "Volatility30d"
        public string OrderDirection { get; set; } = "ASC"; // "ASC" ou "DESC"
    }
}
