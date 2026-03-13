using System.Text;
using CoverletterGenerator.Data;
using CoverletterGenerator.Entities;
using MediatR;
using UglyToad.PdfPig;

namespace CoverletterGenerator.Features.UploadCV
{
    public class UploadCVHandler(AppDbContext dbContext)
        : IRequestHandler<UploadCVCommand, UploadCVResult>
    {
        public async Task<UploadCVResult> Handle(
            UploadCVCommand request,
            CancellationToken cancellationToken
        )
        {
            var content = await ExtractTextAsync(request.File);

            var cv = new CV
            {
                Id = Guid.NewGuid(),
                Content = content,
                UserId = request.UserId,
            };

            dbContext.CVs.Add(cv);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UploadCVResult(cv.Id);
        }

        private static async Task<string> ExtractTextAsync(IFormFile file)
        {
            var contentType = file.ContentType.ToLowerInvariant();
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (contentType == "application/pdf" || extension == ".pdf")
            {
                using var stream = file.OpenReadStream();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);

                using var pdf = PdfDocument.Open(ms.ToArray());
                var sb = new StringBuilder();
                foreach (var page in pdf.GetPages())
                    sb.AppendLine(page.Text);

                return sb.ToString().Trim();
            }

            using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            return (await reader.ReadToEndAsync()).Trim();
        }
    }
}
