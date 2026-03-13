using System.Text;
using Microsoft.Playwright;
using UglyToad.PdfPig;

namespace CoverletterGenerator.Infrastructure.PDF
{
    public class PdfService : IPdfService
    {
        public Task<string> GenerateHtmlFromPdf(byte[] pdfBytes)
        {
            using var pdf = PdfDocument.Open(pdfBytes);
            var sb = new StringBuilder();
            foreach (var page in pdf.GetPages())
                sb.AppendLine(page.Text);
            return Task.FromResult(sb.ToString().Trim());
        }

        public async Task<byte[]> GeneratePdfFromHtml(string html)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();

            await page.SetContentAsync(
                html,
                new PageSetContentOptions { WaitUntil = WaitUntilState.NetworkIdle }
            );

            return await page.PdfAsync(
                new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    Margin = new Margin
                    {
                        Top = "0",
                        Bottom = "0",
                        Left = "0",
                        Right = "0",
                    },
                }
            );
        }
    }
}
