using System.Security.Claims;
using MediatR;

namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public static class GenerateCoverLetterEndpoint
    {
        public static void MapGenerateCoverLetterEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/api/cover-letter",
                    async (GenerateCoverLetterRequest request, ClaimsPrincipal user, IMediator mediator) =>
                    {
                        var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                        var command = new GenerateCoverLetterCommand(request.SeekJobUrl, request.CVId, userId);
                        var result = await mediator.Send(command);
                        return Results.File(result.PdfBytes, "application/pdf", result.FileName);
                    }
                )
                .WithName("GenerateCoverLetter")
                .WithOpenApi()
                .RequireAuthorization();
        }
    }

    public record GenerateCoverLetterRequest(string SeekJobUrl, Guid CVId);
}
