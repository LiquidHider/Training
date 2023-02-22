using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using Training.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Training.Web.Converters;

namespace Training.Web.Services
{
    public class PDFReportGenerationService : IDocumentGenerationService
    {
        private readonly ApplicationDBContext _db;

        public PDFReportGenerationService( ApplicationDBContext db)
        {
            _db = db;
        }

        public void GenerateDocument(string path, string fileName)
        {
            Document Report = new Document();
            ListToDataTableConverter converter = new ListToDataTableConverter();

            string filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            PdfWriter.GetInstance(Report, new FileStream(filePath, FileMode.Create));
            Report.Open();

            var reportTable = converter.ToDataTable(_db.Goods.ToList()); //dataSet.Tables[0];

            PdfPTable table = new PdfPTable(reportTable.Columns.Count);
            PdfPCell cell = new PdfPCell(new Phrase("Report"));
            cell.Colspan = reportTable.Columns.Count;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);
            for (int j = 0; j < reportTable.Columns.Count; j++)
            {
                cell = new PdfPCell(new Phrase(new Phrase(reportTable.Columns[j].ColumnName)));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }
            for (int j = 0; j < reportTable.Rows.Count; j++)
            {
                for (int k = 0; k < reportTable.Columns.Count; k++)
                {
                    table.AddCell(new Phrase(reportTable.Rows[j][k].ToString()));
                }
            }
            Report.Add(table);
            Report.Close();
        }
    }
}
