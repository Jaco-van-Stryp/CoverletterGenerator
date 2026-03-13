namespace CoverletterGenerator.Infrastructure.PDF
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfFromHtml(string Html);
        Task<string> GenerateHtmlFromPdf(byte[] pdfBytes);
    }
}
