namespace ExchangePulse.Application.Interfaces
{
    public interface IReportExporter
    {
        /// <summary>
        /// Exporta um relatório para JSON.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO do relatório.</typeparam>
        /// <param name="data">Coleção de dados a exportar.</param>
        /// <returns>String JSON formatada.</returns>
        string ExportToJson<T>(IEnumerable<T> data);

        /// <summary>
        /// Exporta um relatório para CSV.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO do relatório.</typeparam>
        /// <param name="data">Coleção de dados a exportar.</param>
        /// <returns>String CSV formatada.</returns>
        string ExportToCsv<T>(IEnumerable<T> data);

        /// <summary>
        /// Exporta um relatório para PDF.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO do relatório.</typeparam>
        /// <param name="data">Coleção de dados a exportar.</param>
        /// <param name="title">Título do relatório.</param>
        /// <returns>Array de bytes representando o PDF.</returns>
        byte[] ExportToPdf<T>(IEnumerable<T> data, string title);
    }
}
