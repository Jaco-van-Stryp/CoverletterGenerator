using System.Security.Claims;
using MediatR;

namespace CoverletterGenerator.Features.UploadCV
{
    public static class UploadCVEndpoint
    {
        public static void MapUploadCVEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/api/cv",
                    async (IFormFile file, ClaimsPrincipal user, IMediator mediator) =>
                    {
                        var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                        var result = await mediator.Send(new UploadCVCommand(file, userId));
                        return Results.Ok(result);
                    }
                )
                .WithName("UploadCV")
                .WithOpenApi()
                .RequireAuthorization()
                .DisableAntiforgery();
        }
    }
}
