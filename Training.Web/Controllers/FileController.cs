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
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public FileController(ApplicationDBContext db, IConfiguration configuration, IWebHostEnvironment env)
        {
            _db = db;
            _configuration = configuration;
            _env = env;
        }

        [HttpGet, ActionName("Download")]
        public ActionResult Download()
        {
            Generate();
            string fileName = "Report.pdf";
            var contentFolder = Path.Combine(_env.ContentRootPath, "Content");
            var filePath = Path.Combine(contentFolder, fileName);
            

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return  File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

          
        }

        public void Generate()
        {
            Document Report = new Document();
            var contentFolder = Path.Combine(_env.ContentRootPath, "Content");
            var filePath = Path.Combine(contentFolder, "Report.pdf");
            if (!Directory.Exists(contentFolder))
            {
                Directory.CreateDirectory(contentFolder);
            }
            if (System.IO.File.Exists(filePath)) 
            {
                System.IO.File.Delete(filePath);
            }

            PdfWriter.GetInstance(Report, new FileStream(filePath, FileMode.Create));
            Report.Open();
            IConfigurationSection section = _configuration.GetSection("ConnectionStrings");
            string conn_str = section["DefaultConnection"];
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT [Id], [Name],[Description], [AppraisedValue], [CategoryId], [Status] FROM [LombartDb].[dbo].[Goods]", conn_str);
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
