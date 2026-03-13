using MediatR;

namespace CoverletterGenerator.Features.LoginUser
{
    public record LoginUserCommand(string Email, string Password)
        : IRequest<LoginUserResult>;
}
