using ExchangePulse.Domain.Entities;

namespace ExchangePulse.Application.Aggregators
{
    public static class ReportAggregator
    {
        public static decimal ComputeMonthlyVolatility(IEnumerable<ExchangeMetric> metrics)
            => metrics.Average(m => m.Volatility30d);

        public static (decimal max, decimal min, decimal drawdown) ComputeMonthlyDrawdown(IEnumerable<ExchangeMetric> metrics)
        {
            var dd = metrics.Average(m => m.Drawdown);
            var max = metrics.Max(m => m.SharpeDaily);   // placeholder: ajuste para preço médio se necessário
            var min = metrics.Min(m => m.SharpeDaily);   // placeholder: ajuste para preço médio se necessário
            return (max, min, dd);
        }

        public static (decimal varEmp, decimal varCf) ComputeMonthlyVar(IEnumerable<ExchangeMetric> metrics)
        {
            var emp = metrics.Average(m => m.VaREmpirical95);
            var cf = metrics.Average(m => m.VaRCornishFisher95);
            return (emp, cf);
        }
    }
}
