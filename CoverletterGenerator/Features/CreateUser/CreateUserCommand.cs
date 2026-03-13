using MediatR;

namespace CoverletterGenerator.Features.CreateUser
{
    public record CreateUserCommand(string Name, string Email, string Password)
        : IRequest<CreateUserResult>;
}
