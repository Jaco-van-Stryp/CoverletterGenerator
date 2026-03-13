using MediatR;

namespace CoverletterGenerator.Features.CreateUser
{
    public static class CreateUserEndpoint
    {
        public static void MapCreateUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/api/users/register",
                    async (CreateUserCommand command, IMediator mediator) =>
                    {
                        var result = await mediator.Send(command);
                        return Results.Ok(result);
                    }
                )
                .WithName("RegisterUser")
                .WithOpenApi()
                .AllowAnonymous();
        }
    }
}
