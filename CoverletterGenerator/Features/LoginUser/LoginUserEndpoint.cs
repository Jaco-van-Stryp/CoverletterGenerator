using MediatR;

namespace CoverletterGenerator.Features.LoginUser
{
    public static class LoginUserEndpoint
    {
        public static void MapLoginUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/api/users/login",
                    async (LoginUserCommand command, IMediator mediator) =>
                    {
                        var result = await mediator.Send(command);
                        return Results.Ok(result);
                    }
                )
                .WithName("LoginUser")
                .WithOpenApi()
                .AllowAnonymous();
        }
    }
}
