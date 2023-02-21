using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using Training.Web.Data;

namespace Training.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _configuration;
        public FileController(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost, ActionName("Download")]
        public ActionResult Download()
        {
            Generate();
            string filePath = "/Content/Report.pdf";
            string fileName = "Report.pdf";

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return  File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //< a href = "@Url.Action("DownloadFile", "ControllerName")" > Download File </ a >
        }

        public void Generate()
        {
            iTextSharp.text.Document Report = new iTextSharp.text.Document();
            PdfWriter.GetInstance(Report, new FileStream("Report.pdf", FileMode.Create));
            Report.Open();
            IConfigurationSection section = _configuration.GetSection("ConnectionStrings");
            string conn_str = section["DefaultConnection"];
            SqlDataAdapter dataAdapter = new SqlDataAdapter("!!!!!!!!!!!!!!!!!", conn_str);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            var reportTable = dataSet.Tables[0];
            PdfPTable table = new PdfPTable(reportTable.Columns.Count);
            PdfPCell cell = new PdfPCell(new Phrase("Report"));
            cell.Colspan = reportTable.Columns.Count;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);
            for (int j = 0; j < reportTable.Columns.Count; j++)
            {
                cell = new PdfPCell(new Phrase(new Phrase(reportTable.Columns[j].ColumnName)));
                cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
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
