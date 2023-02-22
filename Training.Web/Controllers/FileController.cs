using Microsoft.AspNetCore.Mvc;
using Training.Web.Data;
using Training.Web.Services;

namespace Training.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDocumentGenerationService _docGenService;
        public FileController(IWebHostEnvironment env, IDocumentGenerationService docGenService)
        {
            _env = env;
            _docGenService = docGenService;
        }

        [HttpGet, ActionName("Download")]
        public ActionResult Download()
        {
            var contentFolder = Path.Combine(_env.ContentRootPath, "Content");
            string fileName = "Report.pdf";
            var filePath = Path.Combine(contentFolder, fileName);

            _docGenService.GenerateDocument(contentFolder, fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
