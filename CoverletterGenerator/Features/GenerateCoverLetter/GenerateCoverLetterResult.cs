namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public readonly record struct GenerateCoverLetterResult(byte[] PdfBytes, string FileName);
}
