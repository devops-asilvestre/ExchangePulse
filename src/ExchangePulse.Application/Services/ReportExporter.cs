using ExchangePulse.Application.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace ExchangePulse.Application.Services
{
    public class ReportExporter : IReportExporter
    {
        public string ExportToJson<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(data, options);
        }

        public string ExportToCsv<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Cabeçalho
            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));

            // Linhas
            foreach (var item in data)
            {
                var values = props.Select(p => p.GetValue(item, null)?.ToString()?.Replace(",", ";") ?? "");
                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString();
        }

        public byte[] ExportToPdf<T>(IEnumerable<T> data, string title)
        {
            using var ms = new MemoryStream();
            var doc = new iTextSharp.text.Document(PageSize.A4);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Título
            var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            doc.Add(new Paragraph(title, fontTitle));
            doc.Add(new Paragraph("\n"));

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var table = new PdfPTable(props.Length);

            // Cabeçalho
            foreach (var p in props)
            {
                var cell = new PdfPCell(new Phrase(p.Name, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
            }

            // Linhas
            foreach (var item in data)
            {
                foreach (var p in props)
                {
                    var value = p.GetValue(item, null)?.ToString() ?? "";
                    table.AddCell(new Phrase(value, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                }
            }

            doc.Add(table);
            doc.Close();

            return ms.ToArray();
        }
    }
}
