using iTextSharp.text;
using iTextSharp.text.pdf;
using Training.Web.Data;
using Training.Web.Converters;
using System.Drawing;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Training.Web.Models;

namespace Training.Web.Services
{
    public class PDFReportGenerationService : IDocumentGenerationService
    {
        private readonly ApplicationDBContext _db;
        private readonly IGoodsService _goodsService;

        public PDFReportGenerationService( ApplicationDBContext db, IGoodsService goodsService)
        {
            _db = db;
            _goodsService = goodsService;
        }

        public void GenerateDocument(string path, string fileName)
        {
            //
            IEnumerable<Category> objCategoryList =  _db.Categories.ToList();
            IEnumerable<RegisteredInvoice> registeredInvoices =  _db.RegisteredInvoices.Include(x => x.Good).Include(x => x.Good.Category).ToList();
            IEnumerable<Good> objGoodList = registeredInvoices.Select(x => x.Good).ToList();
            foreach (var item in registeredInvoices)
            {
                _goodsService.CheckStorageExpirationDate(item);
            }

            var objGoodsReportModel = objGoodList.Select(p =>
                new GoodsReportModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status,
                    AppraisedValue = p.AppraisedValue,
                    Commision = Math.Round((p.AppraisedValue * p.Category.Commision) / 100, 2),
                    Category = p.Category.Name,
                    ReceiptDate = registeredInvoices.FirstOrDefault(x => x.GoodId == p.Id).ReceiptDate,
                    StorageDate = registeredInvoices.FirstOrDefault(x => x.GoodId == p.Id).StorageDate
                }
            ).ToList();
            //
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

            PdfWriter writer  = PdfWriter.GetInstance(Report, new FileStream(filePath, FileMode.Create));

            Report.Open();

            Paragraph title = new Paragraph("Pawnshop - Fourty Two");
            title.Alignment = Element.ALIGN_RIGHT;
            Paragraph date = new Paragraph($"Date of formation {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
            date.Alignment = Element.ALIGN_RIGHT;
            Report.Add(title);
            Report.Add(date);

            Paragraph namedoc = new Paragraph($"\nREPORT on the work of the pawn shop\n");
            namedoc.Alignment = Element.ALIGN_CENTER;
            Report.Add(namedoc);

            //TABLE
            var reportTable = converter.ToDataTable(objGoodsReportModel);
            PdfPTable table = new PdfPTable(reportTable.Columns.Count);
            PdfPCell cell = new PdfPCell(new Phrase("\nGoods\n"));
            table.SetTotalWidth(new float[] { 30f, 60f, 60f, 60f, 65f, 60f, 75f, 75f });
            table.LockedWidth = true;
            cell.Colspan = reportTable.Columns.Count;
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
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

            PdfFormField fullNameField = PdfFormField.CreateTextField(writer, false, false, 0);
            PdfFormField signatureField = PdfFormField.CreateTextField(writer, false, false, 0);
            // Настройка полей формы
            fullNameField.SetWidget(new iTextSharp.text.Rectangle(100, 700, 300, 720), PdfAnnotation.HIGHLIGHT_INVERT);
            fullNameField.FieldName = "fullName";
            fullNameField.SetFieldFlags(PdfFormField.FF_READ_ONLY);
            fullNameField.ValueAsString = "";
            Report.Add(new Paragraph("\n\nCashier    ______________________________"));
            writer.AddAnnotation(fullNameField);

            signatureField.SetWidget(new iTextSharp.text.Rectangle(100, 650, 300, 670), PdfAnnotation.HIGHLIGHT_INVERT);
            signatureField.FieldName = "signature";
            signatureField.SetFieldFlags(PdfFormField.FF_READ_ONLY);
            signatureField.ValueAsString = "";
            Report.Add(new Paragraph("\nSignature ______________________________"));
            writer.AddAnnotation(signatureField);

            Report.Close();
        }
    }
}
