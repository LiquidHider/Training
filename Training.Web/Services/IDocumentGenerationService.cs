using iTextSharp.text;

namespace Training.Web.Services
{
    public interface IDocumentGenerationService
    {
        void GenerateDocument(string path, string fileName);
    }
}
