using System.Text;
using System.Text.Json;
using CoverletterGenerator.Data;
using CoverletterGenerator.Entities;
using CoverletterGenerator.Infrastructure.Anthropic;
using MediatR;
using UglyToad.PdfPig;

namespace CoverletterGenerator.Features.UploadCV
{
    public class UploadCVHandler(AppDbContext dbContext, IAnthropicService anthropicService)
        : IRequestHandler<UploadCVCommand, UploadCVResult>
    {
        public async Task<UploadCVResult> Handle(
            UploadCVCommand request,
            CancellationToken cancellationToken
        )
        {
            var content = await ExtractTextAsync(request.File);

            var jsonTemplate = """
                {
                  "name": "...",
                  "email": "...",
                  "phone": "...",
                  "location": "...",
                  "linkedin": "...",
                  "github": "...",
                  "website": "..."
                }
                """;
            var extractionPrompt = $"""
                Extract the candidate's contact details from this CV text and return them as JSON only.
                Return exactly this structure (use null for any field not found):
                {jsonTemplate}
                Copy all values character-for-character from the CV text. Do not correct, reformat, or normalise any value.
                Return JSON only. No preamble, no explanation, no code fences.

                CV TEXT:
                {content}
                """;

            var extractionResponse = await anthropicService.GenerateAIResponse(extractionPrompt);

            var contactJson = extractionResponse.Text.Trim();
            if (contactJson.StartsWith("```"))
            {
                contactJson = contactJson[(contactJson.IndexOf('\n') + 1)..];
                var endFence = contactJson.LastIndexOf("```");
                if (endFence >= 0)
                    contactJson = contactJson[..endFence].TrimEnd();
            }

            ContactDetails? contactDetails = null;
            try
            {
                contactDetails = JsonSerializer.Deserialize<ContactDetails>(
                    contactJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (JsonException) { }

            var cv = new CV
            {
                Id = Guid.NewGuid(),
                Content = content,
                UserId = request.UserId,
                Name = contactDetails?.Name,
                Email = contactDetails?.Email,
                Phone = contactDetails?.Phone,
                Location = contactDetails?.Location,
                LinkedIn = contactDetails?.LinkedIn,
                GitHub = contactDetails?.GitHub,
                Website = contactDetails?.Website,
            };

            dbContext.CVs.Add(cv);

            dbContext.AiTokenUsages.Add(new AiTokenUsage
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Model = extractionResponse.Model,
                InputTokens = extractionResponse.InputTokens,
                OutputTokens = extractionResponse.OutputTokens,
                CacheCreationInputTokens = extractionResponse.CacheCreationInputTokens,
                CacheReadInputTokens = extractionResponse.CacheReadInputTokens,
            });

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

        private record ContactDetails(
            string? Name,
            string? Email,
            string? Phone,
            string? Location,
            string? LinkedIn,
            string? GitHub,
            string? Website
        );
    }
}
