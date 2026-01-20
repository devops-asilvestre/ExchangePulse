namespace ExchangePulse.Application.Interfaces
{
    public interface IBcbDataFetcher
    {
        /// <summary>
        /// Retorna a taxa SELIC diária para a data informada.
        /// </summary>
        Task<decimal> GetSelicAsync(DateTime date);

        /// <summary>
        /// Retorna o IPCA mensal para a data informada.
        /// </summary>
        Task<decimal> GetIpcaAsync(DateTime date);
    }
}
